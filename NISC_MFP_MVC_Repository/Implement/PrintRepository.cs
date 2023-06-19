using AutoMapper;
using AutoMapper.QueryableExtensions;
using NISC_MFP_MVC_Common;
using NISC_MFP_MVC_Repository.DB;
using NISC_MFP_MVC_Repository.DTOs.InitialValue.Print;
using NISC_MFP_MVC_Repository.DTOs.OutputReport;
using NISC_MFP_MVC_Repository.DTOs.Print;
using NISC_MFP_MVC_Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace NISC_MFP_MVC_Repository.Implement
{
    public class PrintRepository : IPrintRepository
    {
        protected MFP_DB db { get; private set; }
        private readonly Mapper _mapper;

        public PrintRepository()
        {
            db = new MFP_DB();
            _mapper = InitializeAutoMapper();
        }

        public void Insert(InitialPrintRepoDTO instance)
        {
            //NOP
        }

        public IQueryable<InitialPrintRepoDTO> GetAll()
        {
            return db.tb_logs_print.AsNoTracking().ProjectTo<InitialPrintRepoDTO>(_mapper.ConfigurationProvider);
        }

        public IQueryable<InitialPrintRepoDTONeed> GetRecord(InitialOutputReportRepoDTO initialOutputReportRepoDTO)
        {
            IQueryable<tb_logs_print> result = db.tb_logs_print.AsNoTracking();

            if (!string.IsNullOrEmpty(initialOutputReportRepoDTO.reportColor)) result = result.Where(d => d.page_color.Equals(initialOutputReportRepoDTO.reportColor));

            if (!string.IsNullOrEmpty(initialOutputReportRepoDTO.deptId)) result = result.Where(d => d.dept_id.Equals(initialOutputReportRepoDTO.deptId));

            List<string> usageTypeList = initialOutputReportRepoDTO.usage_type.ToCharArray().Select(c => c.ToString()).ToList();
            if (!string.IsNullOrEmpty(initialOutputReportRepoDTO.usage_type)) result = result.Where("@0.Contains(usage_type)", usageTypeList);

            if (!string.IsNullOrEmpty(initialOutputReportRepoDTO.userId)) result = result.Where(d => d.user_id.Equals(initialOutputReportRepoDTO.userId));

            if (!string.IsNullOrEmpty(initialOutputReportRepoDTO.mfpIp)) result = result.Where(d => d.mfp_ip.Equals(initialOutputReportRepoDTO.mfpIp));

            string[] postDateRange = initialOutputReportRepoDTO.date.Split('~');
            DateTime startDate = Convert.ToDateTime(postDateRange[0]);
            DateTime endDate = Convert.ToDateTime(postDateRange[1]);
            result = result.Where(print => print.print_date >= startDate && print.print_date <= endDate);

            IQueryable<InitialPrintRepoDTONeed> prints = result.Select(p => new InitialPrintRepoDTONeed
            {
                mfp_name = p.mfp_name,
                user_id = p.user_id,
                user_name = p.user_name,
                dept_id = p.dept_id,
                dept_name = p.dept_name,
                card_id = p.card_id,
                card_type = p.card_type == "0" ? "遞減" : "遞增",
                usage_type = p.usage_type == "C" ? "影印" : p.usage_type == "P" ? "列印" : p.usage_type == "S" ? "掃描" : p.usage_type == "F" ? "傳真" : "",
                page_color = p.page_color == "C" ? "C(彩色)" : "M(單色)",
                page = p.page,
                value = p.value,
                print_date = p.print_date,
                document_name = p.document_name,
                file_path = p.file_path.ToUpper() == "NULL" ? null : p.file_path,
                file_name = p.file_name.ToUpper() == "NULL" ? null : p.file_name,
                serial = p.serial
            });
            return prints;
        }

        public IQueryable<InitialPrintRepoDTO> GetAll(DataTableRequest dataTableRequest)
        {
            string[] columns = {
                "mfp_name",
                "user_name",
                "dept_name",
                "card_id",
                "card_type",
                "usage_type",
                "page_color",
                "page",
                "value",
                "print_date",
                "document_name"
            };
            string[] searches = {
                dataTableRequest.ColumnSearch_0,
                dataTableRequest.ColumnSearch_1,
                dataTableRequest.ColumnSearch_2,
                dataTableRequest.ColumnSearch_3,
                dataTableRequest.ColumnSearch_4,
                dataTableRequest.ColumnSearch_5,
                dataTableRequest.ColumnSearch_6,
                dataTableRequest.ColumnSearch_7,
                dataTableRequest.ColumnSearch_8,
                dataTableRequest.ColumnSearch_9,
                dataTableRequest.ColumnSearch_10
            };


            IQueryable<InitialPrintRepoDTO> tb_Logs_Prints = db.tb_logs_print.AsNoTracking()
                .Select(p => new InitialPrintRepoDTONeed
                {
                    mfp_name = p.mfp_name,
                    user_name = p.user_name,
                    dept_name = p.dept_name,
                    card_id = p.card_id,
                    card_type = p.card_type == "0" ? "遞減" : "遞增",
                    usage_type = p.usage_type == "C" ? "影印" : p.usage_type == "P" ? "列印" : p.usage_type == "S" ? "掃描" : p.usage_type == "F" ? "傳真" : "",
                    page_color = p.page_color == "C" ? "C(彩色)" : "M(單色)",
                    value = p.value,
                    page = p.page,
                    print_date = p.print_date,
                    document_name = p.document_name,
                    file_path = p.file_path.ToUpper() == "NULL" ? null : p.file_path,
                    file_name = p.file_name.ToUpper() == "NULL" ? null : p.file_name,
                    serial = p.serial
                });

            //GlobalSearch
            tb_Logs_Prints = GetWithGlobalSearch(tb_Logs_Prints, dataTableRequest.GlobalSearchValue);//0

            //Column Search
            tb_Logs_Prints = GetWithColumnSearch(tb_Logs_Prints, columns, searches);//0~1

            //-----------------Performance BottleNeck-----------------
            dataTableRequest.RecordsFilteredGet = tb_Logs_Prints.Count();
            //-----------------Performance BottleNeck-----------------

            tb_Logs_Prints = tb_Logs_Prints.OrderBy(dataTableRequest.SortColumnName + " " + dataTableRequest.SortDirection);//0

            tb_Logs_Prints = tb_Logs_Prints.Skip(() => dataTableRequest.Start).Take(() => dataTableRequest.Length);

            return tb_Logs_Prints;
        }

        public IQueryable<InitialPrintRepoDTO> GetWithGlobalSearch(IQueryable<InitialPrintRepoDTO> source, string search)
        {
            source = source
                .Where(p =>
                    ((!string.IsNullOrEmpty(p.mfp_name)) && p.mfp_name.Contains(search)) ||
                    ((!string.IsNullOrEmpty(p.user_name)) && p.user_name.Contains(search)) ||
                    ((!string.IsNullOrEmpty(p.dept_name)) && p.dept_name.Contains(search)) ||
                    ((!string.IsNullOrEmpty(p.card_id)) && p.card_id.Contains(search)) ||
                    ((!string.IsNullOrEmpty(p.card_type)) && p.card_type.Contains(search)) ||
                    ((!string.IsNullOrEmpty(p.usage_type)) && p.usage_type.Contains(search)) ||
                    ((!string.IsNullOrEmpty(p.page_color)) && p.page_color.Contains(search)) ||
                    ((p.page != null) && p.page.ToString().Contains(search)) ||
                    ((p.value != null) && p.value.ToString().Contains(search)) ||
                    ((!string.IsNullOrEmpty(p.print_date.ToString())) && p.print_date.ToString().Contains(search)) ||
                    ((!string.IsNullOrEmpty(p.document_name)) && p.document_name.Contains(search)));

            return source;
        }

        public IQueryable<InitialPrintRepoDTO> GetWithColumnSearch(IQueryable<InitialPrintRepoDTO> source, string[] columns, string[] searches)
        {
            for (int i = 0; i < columns.Length; i++)
            {
                if (columns[i] == "print_date")
                {
                    if (searches[i].Contains("~"))
                    {
                        string[] postDateRange = searches[i].Split('~');
                        DateTime startDate = Convert.ToDateTime(postDateRange[0]);
                        DateTime endDate = Convert.ToDateTime(postDateRange[1]);
                        source = source.Where(print => print.print_date >= startDate && print.print_date <= endDate);
                    }
                    else
                    {
                        source = source.Where(columns[i] + ".ToString().ToUpper().Contains" + "(\"" + searches[i].ToString().ToUpper() + "\")");
                    }
                }
                else if (columns[i] == "usage_type")
                {
                    if (searches[i] == "AdvancedEmpty")
                    {
                        source = Enumerable.Empty<InitialPrintRepoDTO>().AsQueryable();
                    }
                    else
                    {
                        List<string> operationList = searches[i].Split(',').ToList();
                        source = operationList.Count == 1 ?
                            source.Where(columns[i] + ".ToString().ToUpper().Contains" + "(\"" + searches[i].ToString().ToUpper() + "\")") :
                            source.Where("p => @0.Any(s => p.usage_type.Contains(s))", operationList);

                    }
                }
                else if (columns[i] == "dept_name")
                {
                    if (searches[i] == "AdvancedEmpty")
                    {
                        source = Enumerable.Empty<InitialPrintRepoDTO>().AsQueryable();
                    }
                    else
                    {
                        List<string> departmentList = searches[i].Split(',').ToList();
                        source = departmentList.Count == 1 ?
                            source.Where(columns[i] + ".ToString().ToUpper().Contains" + "(\"" + searches[i].ToString().ToUpper() + "\")") :
                            source.Where("p => @0.Any(s => p.dept_name.Contains(s))", departmentList);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(searches[i]))
                    {
                        source = source.Where(columns[i] + "!=null &&" + columns[i] + ".ToString().ToUpper().Contains(@0)", searches[i].ToString().ToUpper());
                    }
                }
            }
            return source;
        }

        public InitialPrintRepoDTO Get(string column, string value, string operation)
        {
            tb_logs_print result = db.tb_logs_print.Where(column + operation, value).AsNoTracking().FirstOrDefault();
            return _mapper.Map<tb_logs_print, InitialPrintRepoDTO>(result);
        }

        public void Update(InitialPrintRepoDTO instance)
        {
            var dataModel = _mapper.Map<InitialPrintRepoDTO, tb_logs_print>(instance);
            this.db.Entry(dataModel).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void Delete(InitialPrintRepoDTO instance)
        {
            //NOP
        }

        public void SaveChanges()
        {
            this.db.SaveChanges();
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.db != null)
                {
                    this.db.Dispose();
                    this.db = null;
                }
            }
        }

        /// <summary>
        /// 建立AutoMapper配置
        /// </summary>
        /// <returns></returns>
        private Mapper InitializeAutoMapper()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            return new Mapper(config);
        }
    }
}
