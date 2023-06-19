using IniParser;
using IniParser.Model;
using NISC_MFP_MVC_Common;
using NISC_MFP_MVC_Repository.DB;
using NISC_MFP_MVC_Repository.Implement;
using NISC_MFP_MVC_Repository.Interface;
using NISC_MFP_MVC_Service.DTOs.UserAreasInfo.Print;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;

namespace NISC_MFP_MVC_Service.Implement
{
    public class PrintJobService
    {
        private readonly IDocumentManagementRepository _documentManagementRepository;
        private readonly PrintPriceRepository _printPriceRepository;

        public PrintJobService()
        {
            _documentManagementRepository = new DocumentManagementRepository();
            _printPriceRepository = new PrintPriceRepository();
        }

        public PrintJobsModel GetPrintJob(string document_uid)
        {
            doc_mng document = _documentManagementRepository.GetAll().FirstOrDefault(d => d.doc_uid.Equals(document_uid));

            PrintJobsModel result = new PrintJobsModel
            {
                file = document.doc_uid,
                date = (document.ntime ?? DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss"),
                document = document.doc_name,
                pages = document.page_count ?? 0,
                color = (document.bc_print ?? 0) == 0 ? "<b>黑白</b>" : "<b class='rainbow-text'>彩色</b>",
                //此筆列印工作所需點數
                value = (document.page_count ?? 0) * (_printPriceRepository.GetAll().FirstOrDefault(p =>
                    p.color.Equals(((document.bc_print ?? 0) == 0 ? "M" : "C")) &&
                    p.page_size.Equals(document.page_size ?? "")).price ?? 0),
                size = document.page_size
            };

            return result;
        }

        /// <summary>
        /// 取得或刪除最近一小時內待影列印之紀錄 - User
        /// </summary>
        /// <param name="dataTableRequest">DataTable Request Parameter</param>
        /// <param name="user_id">user_id</param>
        /// <param name="documentUid">doc_uid</param>
        /// <returns></returns>
        public List<PrintJobsModel> GetOrDeleteUserPrintJobs(DataTableRequest dataTableRequest, string user_id, string documentUid = "")
        {
            // 如果documentUid有值，則此Method則變成Delete Print Jobs
            if (documentUid != "")
            {

                if (File.Exists($@"{GlobalVariable.PRINT_TEMP_PATH}/{documentUid}.ps")) File.Delete($@"C:/printfile/{documentUid}.ps");
                if (File.Exists($@"C:/printfile/{documentUid}.pdf")) File.Delete($@"C:/printfile/{documentUid}.pdf");
            }

            // Get Print Jobs Life Cycle - Start
            int jobsLifeCycle = 0;
            string DATABASE_PATH = GlobalVariable.DATABASE_INI_PATH;
            if (File.Exists(DATABASE_PATH))
            {
                FileIniDataParser fileIniDataParser = new FileIniDataParser();
                IniData data = fileIniDataParser.ReadFile(DATABASE_PATH);
                jobsLifeCycle = Convert.ToInt32(TimeSpan.Parse(data["systemSetup"]["printjobAlive"]).TotalSeconds);
            }
            // Get Print Jobs Life Cycle - End

            List<PrintJobsModel> tempDataModel = new List<PrintJobsModel>();
            IList<doc_mng> documents = _documentManagementRepository.GetAll()
                    .Where(d =>
                    d.user_id == user_id &&
                    d.data_source == "P" &&
                    DbFunctions.AddHours((d.ntime ?? DateTime.MinValue), jobsLifeCycle) > DateTime.Now)
                    .ToList();

            foreach (doc_mng d in documents)
            {
                PrintJobsModel model = new PrintJobsModel
                {
                    file = d.doc_uid,
                    date = (d.ntime ?? DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss"),
                    document = d.doc_name,
                    pages = d.page_count ?? 0,
                    color = (d.bc_print ?? 0) == 0 ? "<b>黑白</b>" : "<b class='rainbow-text'>彩色</b>",
                    //此筆列印工作所需點數
                    value = (d.page_count ?? 0) * (_printPriceRepository.GetAll().FirstOrDefault(p =>
                            p.color.Equals(((d.bc_print ?? 0) == 0 ? "M" : "C")) &&
                            p.page_size.Equals(d.page_size ?? ""))
                        .price ?? 0),
                    size = d.page_size
                };

                //TODO
                if (File.Exists($@"C:/printfile/{d.doc_uid}.ps") || File.Exists($@"C:/printfile/{d.doc_uid}.pcl"))
                {
                    tempDataModel.Add(model);
                }
            }
            IQueryable<PrintJobsModel> resultDataModel = tempDataModel.AsQueryable();

            dataTableRequest.RecordsFilteredGet = resultDataModel.Count();
            resultDataModel = resultDataModel.OrderBy(dataTableRequest.SortColumnName + " " + dataTableRequest.SortDirection);
            resultDataModel = resultDataModel.Skip(() => dataTableRequest.Start).Take(() => dataTableRequest.Length);
            List<PrintJobsModel> topTenRecord = resultDataModel.ToList();

            return topTenRecord;
        }

        /// <summary>
        /// 取得或刪除card_id的暫存檔
        /// </summary>
        /// <param name="dataTableRequest">DataTable Request</param>
        /// <param name="card_id">card_id</param>
        /// <param name="documentUid">doc_uid</param>
        /// <returns></returns>
        public List<PrintJobsModel> GetOrDeleteCardPrintJobs(DataTableRequest dataTableRequest, string card_id, string documentUid = "")
        {
            // printTempPath is folder.
            string printTempPath = GlobalVariable.PRINT_TEMP_PATH + $@"/clientTemp/{card_id}";
            if (File.Exists(printTempPath))
            {
                if (documentUid != "")
                {
                    if (File.Exists(printTempPath + $@"/{documentUid}")) File.Delete(printTempPath + $@"/{documentUid}");
                    if (File.Exists(printTempPath + $@"/{documentUid.Replace("log", "prn")}")) File.Delete(printTempPath + $@"/{documentUid.Replace("log", "prn")}");
                }

                List<PrintJobsModel> tempDataModel = new List<PrintJobsModel>();
                IEnumerable<string> files = from file in Directory.EnumerateFiles(printTempPath) select file;
                foreach (string file in files)
                {
                    if (file.Substring(-3) == "log" && File.Exists(printTempPath + $@"/{file.Replace("log", "prn")}"))
                    {
                        using (StreamReader reader = new StreamReader(file))
                        {
                            string line;
                            PrintJobsModel printJobsModel = new PrintJobsModel();
                            while ((line = reader.ReadLine()) != null)
                            {
                                IList<string> list = line.Split('=').ToList();
                                switch (list[0])
                                {
                                    case "dateTime":
                                        printJobsModel.date = list[1];
                                        break;
                                    case "documentName":
                                        byte[] bytes = Encoding.Default.GetBytes(list[1]);
                                        printJobsModel.document = Encoding.UTF8.GetString(bytes);
                                        break;
                                    case "TotalPageNum":
                                        printJobsModel.pages = Convert.ToInt32(list[1]);
                                        break;
                                    case "color":
                                        printJobsModel.color = list[1];
                                        break;
                                    case "paperSize":
                                        printJobsModel.size = list[1];
                                        break;
                                }
                            }

                            printJobsModel.file = documentUid;
                            printJobsModel.date = printJobsModel.date ?? "";
                            printJobsModel.document = printJobsModel.document ?? "";
                            printJobsModel.pages = printJobsModel.pages;
                            printJobsModel.value = printJobsModel.pages * (_printPriceRepository.GetAll()
                                .FirstOrDefault(p =>
                                    p.color.Equals(printJobsModel.color == "0" ? "M" : "C") &&
                                    p.page_size.Equals(printJobsModel.size ?? "")).price ?? 0);
                            printJobsModel.color = printJobsModel.color == "0" ? "<b>黑白</b>" : "<b class='rainbow-text'>彩色</b>";
                            printJobsModel.size = printJobsModel.size ?? "";

                            tempDataModel.Add(printJobsModel);
                        }
                    }
                }
                IQueryable<PrintJobsModel> resultDataModel = tempDataModel.AsQueryable();

                dataTableRequest.RecordsFilteredGet = resultDataModel.Count();
                resultDataModel = resultDataModel.OrderBy(dataTableRequest.SortColumnName + " " + dataTableRequest.SortDirection);
                resultDataModel = resultDataModel.Skip(() => dataTableRequest.Start).Take(() => dataTableRequest.Length);
                List<PrintJobsModel> topTenRecord = resultDataModel.ToList();
                return topTenRecord;
            }
            return null;
        }

        public void Dispose()
        {
            _documentManagementRepository.Dispose();
            _printPriceRepository.Dispose();
        }
    }
}
