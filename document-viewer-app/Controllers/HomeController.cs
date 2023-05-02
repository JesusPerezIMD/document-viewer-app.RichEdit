using document_viewer_app.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace document_viewer_app.Controllers
{
    public class HomeController : Controller
    {

        private readonly IHostApplicationLifetime _appLifetime;

        public HomeController(IHostApplicationLifetime appLifetime)
        {
            _appLifetime = appLifetime;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> DescargarArchivo(string nombreArchivo, string urlDescarga)
        {
            // Crear una instancia de HttpClient
            using var httpClient = new HttpClient();

            // Descargar el archivo y almacenarlo temporalmente en el proyecto
            var rutaArchivo = $"ArchivosTemporales/{nombreArchivo}";
            var respuesta = await httpClient.GetAsync(urlDescarga);
            var contenidoArchivo = await respuesta.Content.ReadAsByteArrayAsync();
            System.IO.File.WriteAllBytes(rutaArchivo, contenidoArchivo);

            // Establecer un método de devolución de llamada para eliminar el archivo cuando se cierre el proyecto
            _appLifetime.ApplicationStopped.Register(() =>
            {
                if (System.IO.File.Exists(rutaArchivo))
                {
                    System.IO.File.Delete(rutaArchivo);
                }
            });

            // Descargar el archivo al navegador del usuario
            var stream = new FileStream(rutaArchivo, FileMode.Open);
            return File(stream, "application/octet-stream", nombreArchivo);
        }
    }
}