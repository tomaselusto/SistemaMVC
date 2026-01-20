using AccesoDatos.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modelos.Models;
using Utilidades;

namespace Sistema.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = DS.RolAdmin)]
    public class CategoriaController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;

        public CategoriaController(IUnidadTrabajo ut)
        {
            _unidadTrabajo=ut;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            Categoria categoria= new Categoria();
            if(id==null)
            {
                //nueva
                categoria.Estado = true;
                return View(categoria);
            }
            //actualizar
            categoria = await _unidadTrabajo.Categoria.Obtener(id.GetValueOrDefault());
            if(categoria==null)
            {
                return NotFound();
            }
            return View(categoria);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Categoria categoria)
        {
            if(ModelState.IsValid)
            {
                if(categoria.Id==0)
                {
                    //nueva
                    await _unidadTrabajo.Categoria.Agregar(categoria);
                    TempData[DS.Exitosa] = "Se creó la Categoria";
                }
                else
                {
                    //actualizar
                    _unidadTrabajo.Categoria.Actualizar(categoria);
                    TempData[DS.Exitosa] = "Se actualizó la Categoria";
                }
                await _unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }
            TempData[DS.Error] = "Hubo un problema al crear la categoria";
            return View(categoria);

        }

        #region API
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var todas = await _unidadTrabajo.Categoria.ObtenerTodos();
            return Json(new {data=todas});
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var catBD = await _unidadTrabajo.Categoria.Obtener(id);
            if (catBD == null)
                return Json(new { success = false, message = "No se pudo borrar la categoria" });

            _unidadTrabajo.Categoria.Remover(catBD);
            await _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Se borró la categoria" });
        }

        [ActionName("validarNombre")]
        public async Task<IActionResult> ValidarNombre(string nombre, int id = 0)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return Json(new { data = false });

            var nombreM = nombre.Trim().ToLower();

            var lista = await _unidadTrabajo.Categoria.ObtenerTodos();

            bool existe;

            if(id==0)
            {
                existe= lista.Any(x=> (x.Nombre ?? "").Trim().ToLower() == nombreM);
            }
            else
            {
                existe= lista.Any( x=>
                            (x.Nombre ?? "").Trim().ToLower() == nombreM &&
                            x.Id == id);
            }

            return Json(new { data = existe });

        }   
            
            
            #endregion
    }
}
