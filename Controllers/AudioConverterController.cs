using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Text;

namespace FFMpegWeb.Controllers
{
    public class AudioConverterController : Controller
    {
        //
        // GET: /AudioConverter/
        public string CorrectInvalidCharactersInFileName(string fileName)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                char[] invalidPathChars = Path.GetInvalidPathChars();

                StringBuilder sb = new StringBuilder(fileName);

                foreach (char ch in invalidPathChars)
                {
                    sb.Replace(ch, '_');
                }

                sb.Replace("*", "_");
                sb.Replace("?", "_");
                sb.Replace("<", "_");
                sb.Replace(">", "_");
                sb.Replace("[", "_");
                sb.Replace("]", "_");
                sb.Replace(":", "_");
                sb.Replace("|", "_");
                sb.Replace(" ", "_");
                sb.Replace("=", "_");
                sb.Replace("&", "_");
                sb.Replace("+", "_");
                sb.Replace(",", "_");
                sb.Replace("!", "_");
                sb.Replace(";", "_");

                return sb.ToString();
            }
            else
            {
                return fileName;
            }
        }

        public string GetInputRootPath()
        {
            return string.Format("{0}Input", Request.PhysicalApplicationPath);
        }

        public string GetInputPath()
        {
            return string.Format("{0}\\{1}", GetInputRootPath(), !string.IsNullOrWhiteSpace(Request.Headers["X-Real-IP"]) ? Request.Headers["X-Real-IP"] : Request.IsLocal ? "LOCAL" : Request.UserHostAddress);
        }

        public string GetOutputRootPath()
        {
            return string.Format("{0}Output", Request.PhysicalApplicationPath);
        }

        public string GetOutputPath()
        {
            return string.Format("{0}\\{1}", GetOutputRootPath(), !string.IsNullOrWhiteSpace(Request.Headers["X-Real-IP"]) ? Request.Headers["X-Real-IP"] : Request.IsLocal ? "LOCAL" : Request.UserHostAddress);
        }

        public string GetOutputRelativePath()
        {
            return string.Format("Output\\{0}", !string.IsNullOrWhiteSpace(Request.Headers["X-Real-IP"]) ? Request.Headers["X-Real-IP"] : Request.IsLocal ? "LOCAL" : Request.UserHostAddress);
        }

        public ActionResult Convert()
        {
            return View();
        }

        /*
        [HttpPost]
        public ActionResult Convert(HttpPostedFileBase file)
        {
            // Verify that the user selected a file
            if (file != null && file.ContentLength > 0)
            {
                // extract only the fielname
                var fileName = Path.GetFileName(file.FileName);
                file.SaveAs("test.mp3");

                // Convert to ContentDisposition
                var cd = new System.Net.Mime.ContentDisposition
                {
                    FileName = fileName,

                    // Prompt the user for downloading; set to true if you want 
                    // the browser to try to show the file 'inline' (display in-browser
                    // without prompting to download file).  Set to false if you 
                    // want to always prompt them to download the file.
                    Inline = true
                };
                Response.AppendHeader("Content-Disposition", cd.ToString());

                // View document
                byte[] data;
                using (BinaryReader br = new BinaryReader(file.InputStream))
                {
                    data = br.ReadBytes(file.ContentLength);
                }
                return File(data, "application/x-unknown");
            }
            else
            {
                return View("Error");
            }
        }
        */

        [HttpPost]
        public ActionResult ConvertToMP3()
        {
            if (0 < Request.Files.Count)
            {
                HttpPostedFileBase fu = Request.Files[0];

                // Verify that the user selected a file
                if (fu != null && fu.ContentLength > 0)
                {
                    string inputPath = GetInputPath();
                    if (!Directory.Exists(inputPath)) Directory.CreateDirectory(inputPath);
                    string outputPath = GetOutputPath();
                    if (!Directory.Exists(outputPath)) Directory.CreateDirectory(outputPath);

                    //Saving the file
                    string fileName = Path.GetFileNameWithoutExtension(CorrectInvalidCharactersInFileName(fu.FileName));
                    string fileTail = DateTime.Now.ToString("_yyyy_MM_dd_HH_mm_ss_fff");

                    string fExtension = Path.GetExtension(fu.FileName);

                    if (200 < (inputPath.Length + fileName.Length + fileTail.Length))
                    {
                        fileName = fileName.Substring(0, 200 - inputPath.Length - fileTail.Length);
                    }

                    string inFile = string.Format("{0}\\{1}{2}{3}", inputPath, fileName, fileTail, fExtension);
                    string outFile = string.Format("{0}\\{1}{2}.{3}", outputPath, fileName, fileTail, "mp3");

                    fu.SaveAs(inFile);
                    
                    //Converting
                    fu.SaveAs(outFile);

                    // Convert to ContentDisposition
                    var cd = new System.Net.Mime.ContentDisposition
                    {
                        FileName = fileName,

                        // Prompt the user for downloading; set to true if you want 
                        // the browser to try to show the file 'inline' (display in-browser
                        // without prompting to download file).  Set to false if you 
                        // want to always prompt them to download the file.
                        Inline = true
                    };
                    Response.AppendHeader("Content-Disposition", cd.ToString());

                    // View document
                    byte[] data = System.IO.File.ReadAllBytes(outFile);

                    Response.Cookies.Add(new HttpCookie("AudioConvertionFinished", "1"));

                    return File(data, "audio/mpeg3;audio/x-mpeg-3;");
                }
                else
                {
                    return View("Error");
                }
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        public ActionResult ConvertToWMA()
        {
            if (0 < Request.Files.Count)
            {
                HttpPostedFileBase fu = Request.Files[0];

                // Verify that the user selected a file
                if (fu != null && fu.ContentLength > 0)
                {
                    string inputPath = GetInputPath();
                    if (!Directory.Exists(inputPath)) Directory.CreateDirectory(inputPath);
                    string outputPath = GetOutputPath();
                    if (!Directory.Exists(outputPath)) Directory.CreateDirectory(outputPath);

                    //Saving the file
                    string fileName = Path.GetFileNameWithoutExtension(CorrectInvalidCharactersInFileName(fu.FileName));
                    string fileTail = DateTime.Now.ToString("_yyyy_MM_dd_HH_mm_ss_fff");

                    string fExtension = Path.GetExtension(fu.FileName);

                    if (200 < (inputPath.Length + fileName.Length + fileTail.Length))
                    {
                        fileName = fileName.Substring(0, 200 - inputPath.Length - fileTail.Length);
                    }

                    string inFile = string.Format("{0}\\{1}{2}{3}", inputPath, fileName, fileTail, fExtension);
                    string outFile = string.Format("{0}\\{1}{2}.{3}", outputPath, fileName, fileTail, "wma");

                    fu.SaveAs(inFile);

                    //Converting
                    fu.SaveAs(outFile);

                    // Convert to ContentDisposition
                    var cd = new System.Net.Mime.ContentDisposition
                    {
                        FileName = fileName,

                        // Prompt the user for downloading; set to true if you want 
                        // the browser to try to show the file 'inline' (display in-browser
                        // without prompting to download file).  Set to false if you 
                        // want to always prompt them to download the file.
                        Inline = true
                    };
                    Response.AppendHeader("Content-Disposition", cd.ToString());

                    // View document
                    byte[] data = System.IO.File.ReadAllBytes(outFile);

                    Response.Cookies.Add(new HttpCookie("AudioConvertionFinished", "1"));

                    return File(data, "audio/wma");
                }
                else
                {
                    return View("Error");
                }
            }
            else
            {
                return View("Error");
            }
        }

        public ActionResult Error()
        {
            return View("Error");
        }
    }
}
