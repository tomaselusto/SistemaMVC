using System.Diagnostics;
using System.Numerics;
using AccesoDatos.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Mvc;

using Modelos.Models;

namespace Sistema.Areas.Inventario.Controllers
{
    [Area("Inventario")]
    public class HomeController : Controller
    {

        private readonly IUnidadTrabajo _unidadTrabajo;
        public HomeController(IUnidadTrabajo ut)
        {
            _unidadTrabajo = ut;    
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Producto> prodLista = await _unidadTrabajo.Producto.ObtenerTodos();
            return View(prodLista);
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
