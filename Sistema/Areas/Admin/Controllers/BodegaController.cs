using AccesoDatos.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Mvc;
using Modelos.Models;

namespace Sistema.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BodegaController : Controller
    {
        //llamamos a nuestra unidad de trabajo
        private readonly IUnidadTrabajo _unidadTrabajo;
        public BodegaController(IUnidadTrabajo ut)
        {
             _unidadTrabajo= ut;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task <IActionResult> Upsert(int? id)
        {
            // int? puede ser nulo 
            //upsert-> de update/insert
            Bodega bodega = new Bodega();
            if(id==null)
            {
                //creamos nueva vodega
                bodega.Estado = true;
                return View(bodega);
            }
            //actualizar bodega
            bodega = await _unidadTrabajo.Bodega.Obtener(id.GetValueOrDefault()); //por si llega nulo el valor del id usamos el getvalueordefault
            if(bodega==null)
            {
                return NotFound();
            }
            return View(bodega);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Bodega bodega)
        {
            if(ModelState.IsValid)
            {
                //que este el modelo correcto en sus propiedades
                if(bodega.Id==0)
                    await _unidadTrabajo.Bodega.Agregar(bodega);
                else
                    _unidadTrabajo.Bodega.Actualizar(bodega);

                await _unidadTrabajo.Guardar(); //actualizar la bd
                return RedirectToAction(nameof(Index));
            }
            return View(bodega);
            
        }

        #region API
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos() //IActionResult no solo retorna una lista sino tamb puede retornar objetos con formato JSON
        {
            var todos = await _unidadTrabajo.Bodega.ObtenerTodos();
            return Json(new {data=todos}); //data es el que tenemos que referenciar desde javascript
        }

        
        #endregion
    }
}
