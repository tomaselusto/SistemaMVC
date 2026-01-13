using AccesoDatos.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Mvc;
using Modelos.Models;
using Utilidades;

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
                {
                    await _unidadTrabajo.Bodega.Agregar(bodega);
                    TempData[DS.Exitosa] = "Bodega creada exitosamente";
                }                    
                else
                {
                    _unidadTrabajo.Bodega.Actualizar(bodega);
                    TempData[DS.Exitosa] = "Bodega actualizada exitosamente";
                }

                await _unidadTrabajo.Guardar(); //actualizar la bd
                return RedirectToAction(nameof(Index));
            }
            TempData[DS.Error] = "No se pudo crear la bodega";
            return View(bodega);
            
        }

        #region API
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos() //IActionResult no solo retorna una lista sino tamb puede retornar objetos con formato JSON
        {
            var todos = await _unidadTrabajo.Bodega.ObtenerTodos();
            return Json(new {data=todos}); //data es el que tenemos que referenciar desde javascript
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var bodegaBD = await _unidadTrabajo.Bodega.Obtener(id);
            if (bodegaBD == null)
            {
                return Json(new { success = false, message = "Error al borrar la bodega, no se encontró" });
            }
                
            _unidadTrabajo.Bodega.Remover(bodegaBD);
            await _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Se borró la bodega" });

        }

        //metodo para validar que no se creen bodegas con el mismo nombre
        [ActionName("validarNombre")] //para referenciar desde JS de la vista upsert
        public async Task<IActionResult> ValidarNombre(string nombre, int id = 0)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return Json(new { data = false });

            var nombreNorm = nombre.Trim().ToLower();

            var lista = await _unidadTrabajo.Bodega.ObtenerTodos();

            bool existe;
            if (id == 0)
            {
                existe = lista.Any(b => (b.Nombre ?? "").Trim().ToLower() == nombreNorm);
            }
            else
            {
                existe = lista.Any(b =>
                    (b.Nombre ?? "").Trim().ToLower() == nombreNorm &&
                    b.Id != id
                );
            }

            return Json(new { data = existe });
        }


        #endregion
    }
}
