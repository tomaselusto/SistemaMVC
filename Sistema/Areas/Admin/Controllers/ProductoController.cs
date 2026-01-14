using AccesoDatos.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Mvc;
using Modelos.Models;
using Modelos.ViewModels;
using Utilidades;

namespace Sistema.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductoController : Controller
    {
        //llamamos a nuestra unidad de trabajo
        private readonly IUnidadTrabajo _unidadTrabajo;
        public ProductoController(IUnidadTrabajo ut)
        {
             _unidadTrabajo= ut;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task <IActionResult> Upsert(int? id)
        {
            ProductoVM prodVM = new ProductoVM()
            {
                Producto = new Producto(),
                CategoriaLista = _unidadTrabajo.Producto.ObtenerTodosDropDownList("Categoria"),
                MarcaLista=_unidadTrabajo.Producto.ObtenerTodosDropDownList("Marca")
            };
            if(id==null)
            {
                //crear nuevo producto
                return View(prodVM);
            }
            else
            {
                prodVM.Producto=await _unidadTrabajo.Producto.Obtener(id.GetValueOrDefault());
                if (prodVM.Producto == null)
                    return NotFound();
                return View(prodVM);
            }

               
        }
        

        #region API
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos() //IActionResult no solo retorna una lista sino tamb puede retornar objetos con formato JSON
        {
            var todos = await _unidadTrabajo.Producto.ObtenerTodos(incluirPropiedades:"Categoria,Marca"); //agregamos los datos de navegación para incluir los otros campos
            return Json(new {data=todos}); //data es el que tenemos que referenciar desde javascript
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var prodBD = await _unidadTrabajo.Producto.Obtener(id);
            if (prodBD == null)
            {
                return Json(new { success = false, message = "Error al borrar la bodega, no se encontró" });
            }
                
            _unidadTrabajo.Producto.Remover(prodBD);
            await _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Se borró el producto" });

        }

        //metodo para validar que no se creen bodegas con el mismo nombre
        [ActionName("validarSerie")] //para referenciar desde JS de la vista upsert
        public async Task<IActionResult> ValidarSerie(string serie, int id = 0)
        {
            if (string.IsNullOrWhiteSpace(serie))
                return Json(new { data = false });

            var serieNorm = serie.Trim().ToLower();

            var lista = await _unidadTrabajo.Producto.ObtenerTodos();

            bool existe;
            if (id == 0)
            {
                existe = lista.Any(b => (b.NumeroSerie ?? "").Trim().ToLower() == serieNorm);
            }
            else
            {
                existe = lista.Any(b =>
                    (b.NumeroSerie ?? "").Trim().ToLower() == serieNorm &&
                    b.Id != id
                );
            }

            return Json(new { data = existe });
        }


        #endregion
    }
}
