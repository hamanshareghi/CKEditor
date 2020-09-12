using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CKEditor.Web.Models;
using Microsoft.AspNetCore.Http;

namespace CKEditor.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile fileUp, string CKEditorFuncNum, string CKEditor,
            string langCode)
        {
            string filePath = String.Empty;
            string vMessage = String.Empty;
            //string vFilePath = String.Empty;
            string vOutput = String.Empty;
            try
            {
                if (fileUp != null)
                {
                    var fileName = Guid.NewGuid() + Path.GetExtension(fileUp.FileName);
                    var directory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Upload/");
                    filePath = Path.Combine(directory, fileName);

                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    await using var stream = new FileStream(filePath, FileMode.Create);
                    await fileUp.CopyToAsync(stream);
                    filePath = Url.Content("/Upload/" + fileName);
                    vMessage = "تصویر با مفقیت ذخیره شد";
                }
            }
            catch
            {
                vMessage = "There was an issue uploading";
            }
            vOutput = @"<html><body><script>window.parent.CKEDITOR.tools.callFunction(" + CKEditorFuncNum + ", \"" + filePath + "\", \"" + vMessage + "\");</script></body></html>";
            return Content(vOutput);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
