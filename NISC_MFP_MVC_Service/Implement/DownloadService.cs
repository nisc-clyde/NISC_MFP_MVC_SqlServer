using NISC_MFP_MVC_Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Web.Administration;
using System.Diagnostics;
using System.Net.Http;
using AutoMapper;
using System.Web;

namespace NISC_MFP_MVC_Service.Implement
{
    public class DownloadService
    {
        public byte[] DownloadDocument(string filePath, string fileName)
        {
            string pdfFile = fileName;
            string[] fileNameSplit = fileName.Split('/');
            if (fileNameSplit.Length > 1)
            {
                fileName = fileNameSplit[1];
            }

            if (fileName.Substring(fileName.Length - 3) == "prn")
            {
                string pclFile = $@"{GlobalVariable.IMAGE_PATH}/{fileName}";
                pdfFile = fileName.Replace("prn", "pdf");
                if (File.Exists($@"{GlobalVariable.IMAGE_PATH}/{pdfFile}"))
                {
                    Process process = new Process();
                    ProcessStartInfo startInfo = new ProcessStartInfo()
                    {
                        WindowStyle = ProcessWindowStyle.Hidden,
                        FileName = "cmd.exe",
                    };
                    process.StartInfo = startInfo;

                    //system("rmdir /s /q ".str_replace("/", "\\",$printTempPath)."\\clientTemp\\".substr($file, 0, 10)."\\temp");//移除 C:/printFile/clientTemp/卡號/temp 資料夾
                    startInfo.Arguments = $@"/C rmdir /s /q {GlobalVariable.PRINT_TEMP_PATH}/clientTemp/{fileName.Substring(0, 10)}/temp";
                    process.Start();
                    //system("c:/printer/pcl2pdf/pc-pdl-to-image.exe -l pcl5 -i ".$pclfile." -o ".$printTempPath."/clientTemp/".substr($file, 0, 10)."/temp");
                    startInfo.Arguments = $@"/C c:/printer/pcl2pdf/pc-pdl-to-image.exe -l pcl5 -i {pclFile} -o {GlobalVariable.PRINT_TEMP_PATH}/clientTemp/{fileName.Substring(0, 10)}/temp";
                    process.Start();
                    //system("rename ".str_replace("/", "\\",$printTempPath)."\\clientTemp\\".substr($file, 0, 10)."\\temp\\*.png *.jpg");
                    startInfo.Arguments = $@"/C rename {GlobalVariable.PRINT_TEMP_PATH}/clientTemp/{fileName.Substring(0, 10)}/temp/*.png *.jpg";
                    process.Start();
                    //system("c:/printer/pcl2pdf/CmdJPGsToPDF.exe ".$printTempPath."/clientTemp/".substr($file, 0, 10)."/temp ".$imagePath."/".$pdffile);
                    startInfo.Arguments = $@"/C c:/printer/pcl2pdf/CmdJPGsToPDF.exe {GlobalVariable.PRINT_TEMP_PATH}/clientTemp/{fileName.Substring(0, 10)}/temp/{GlobalVariable.IMAGE_PATH}/{pdfFile}";
                    process.Start();
                    //system("rmdir /s /q ".str_replace("/", "\\",$printTempPath)."\\clientTemp\\".substr($file, 0, 10)."\\temp");
                    startInfo.Arguments = $@"/C rmdir /s /q {GlobalVariable.PRINT_TEMP_PATH}/clientTemp/{fileName.Substring(0, 10)}/temp";
                    process.Start();
                }
            }


            //自動到.vs/config/applicationhost.config找到<site name="NISC_MFP_MVC" id="2">並Mapping到指定之Virtual Directory
            var iisManager = new ServerManager();
            var mySite = iisManager.Sites.FirstOrDefault(p => p.Name.ToUpper() == "NISC_MFP_MVC");

#if !DEBUG
            if (mySite != null && !mySite.Applications[0].VirtualDirectories.Any(p => p.Path == "/CMImgs"))
            {
                mySite.Applications[0].VirtualDirectories.Add(GlobalVariable.VIRTUAL_IMAGE_PATH, GlobalVariable.IMAGE_PATH);
                iisManager.CommitChanges();
            }
#endif

#if DEBUG
            mySite = iisManager.Sites["WebSite1"];

            if (mySite.Applications[0].VirtualDirectories.All(p => p.Path != GlobalVariable.VIRTUAL_IMAGE_PATH))
            {
                mySite.Applications[0].VirtualDirectories.Add(GlobalVariable.VIRTUAL_IMAGE_PATH, GlobalVariable.IMAGE_PATH);
                iisManager.CommitChanges();
            }
#endif

            var vd = mySite.Applications[0].VirtualDirectories.First(p => p.Path == GlobalVariable.VIRTUAL_IMAGE_PATH);
            var path = $@"{vd.PhysicalPath}/{fileName}";

            if (File.Exists(@"c:/xampp/htdocs/nisc_mfp/CmdPassPDF.exe"))
            {
                Process process = new Process();
                ProcessStartInfo processStartInfo = new ProcessStartInfo();
                string currentPath = HttpContext.Current.Server.MapPath("~/Areas/Admin");
                if (!File.Exists($@"{currentPath}/pdf_temp/"))
                {
                    processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    processStartInfo.FileName = "cmd.exe";
                    processStartInfo.Arguments = $@"/C mkdir {currentPath}/pdf_temp/";
                    process.StartInfo = processStartInfo;
                    process.Start();
                }
                processStartInfo.Arguments =
                    $@"/C c:/xampp/htdocs/nisc_mfp/CmdPassPDF.exe {GlobalVariable.IMAGE_PATH}/{pdfFile} {currentPath}/pdf_temp/{fileName} 123456 1";
                process.Start();
                var fileBytes = File.ReadAllBytes($@"{currentPath}/pdf_temp/{fileName}");
            }
            else
            {
                if (File.Exists(path))
                {
                    var fileBytes = File.ReadAllBytes(path);
                    return fileBytes;
                }
            }
            return null;
        }
    }
}
