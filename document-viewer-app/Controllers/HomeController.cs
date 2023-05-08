using DevExpress.XtraRichEdit;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using document_viewer_app.Models;

namespace document_viewer_app.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> RichEdit(string nombreArchivo = "Documento.docx")
        {
            if (string.IsNullOrEmpty(nombreArchivo))
            {
                // Manejo del caso en que no se proporciona un nombre de archivo
                return Content("Debe proporcionar un nombre de archivo");
            }

            string fileUrl = $"https://bconnectstoragetest.blob.core.windows.net/temp/{nombreArchivo}";
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(fileUrl);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsByteArrayAsync();
                    //var server = new RichEditDocumentServer();
                    //server.LoadDocument(new MemoryStream(content), DocumentFormat.OpenXml);
                    //var model = new DocumentInfo();
                    //model.DocumentBytes = content;
                    //model.DocumentFormat = (int)DevExpress.AspNetCore.RichEdit.DocumentFormat.OpenXml;
                    //model.DocumentName = nombreArchivo;
                    return View(content);
                }
                else
                {
                    return Content("Error al descargar el archivo");
                }
            }
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