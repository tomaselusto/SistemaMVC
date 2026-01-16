using System.Diagnostics;
using System.Numerics;
using AccesoDatos.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Mvc;
using Modelos.Especificaciones;
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
        public IActionResult Index(int pageNumber=1, string busqueda="", string busquedaActual="")
        {
            bool busq = false;
            if(!string.IsNullOrEmpty(busqueda))
            {
                pageNumber = 1;
                busq = true;

            }
            else
            {
                busqueda = busquedaActual;
                busq = false;
            }
            ViewData["BusquedaActual"] = busqueda;


            if (pageNumber < 1)
            {
                pageNumber = 1;
            }

            Parametros param = new Parametros()
            {
                PageNumber = pageNumber,
                PageSize = 4  //que se muestren 4 productos por página
            };

            var resultado = _unidadTrabajo.Producto.ObtenerTodosPaginados(param);

            //para búsqueda
            if(busq)
            {
                resultado = _unidadTrabajo.Producto.ObtenerTodosPaginados(param, p => p.Descripcion.Contains(busqueda));
            }



            //para que quede todo en memoria y manejarlo desde ahí
            ViewData["TotalPaginas"] = resultado.MetaData.TotalPages;
            ViewData["TotalRegistros"] = resultado.MetaData.TotalCount;
            ViewData["PageSize"] = resultado.MetaData.PageSize;
            ViewData["PageNumber"] = pageNumber;
            ViewData["Previo"] = "disabled"; //para el style de las clases se activen o desactiven - boton previo
            ViewData["Siguiente"] = ""; //botón siguiente

            if (pageNumber > 1) { ViewData["Previo"] = ""; }
            if(resultado.MetaData.TotalPages<= pageNumber) { ViewData["Siguiente"] = "disabled"; }

            return View(resultado);
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
