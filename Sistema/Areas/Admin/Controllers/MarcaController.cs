using AccesoDatos.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Mvc;
using Modelos.Models;
using Utilidades;

namespace Sistema.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MarcaController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;

        public MarcaController(IUnidadTrabajo ut)
        {
            _unidadTrabajo = ut;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            Marca marca = new Marca();
            if (id == null)
            {
                //nueva
                marca.Estado = true;
                return View(marca);
            }
            //actualizar
            marca = await _unidadTrabajo.Marca.Obtener(id.GetValueOrDefault());
            if (marca == null)
            {
                return NotFound();
            }
            return View(marca);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Marca marca)
        {
            if (ModelState.IsValid)
            {
                if (marca.Id == 0)
                {
                    //nueva
                    await _unidadTrabajo.Marca.Agregar(marca);
                    TempData[DS.Exitosa] = "Se creó la Marca";
                }
                else
                {
                    //actualizar
                    _unidadTrabajo.Marca.Actualizar(marca);
                    TempData[DS.Exitosa] = "Se actualizó la Marca";
                }
                await _unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }
            TempData[DS.Error] = "Hubo un problema al crear la categoria";
            return View(marca);

        }

        #region API
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var todas = await _unidadTrabajo.Marca.ObtenerTodos();
            return Json(new { data = todas });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var marcaBD = await _unidadTrabajo.Marca.Obtener(id);
            if (marcaBD == null)
                return Json(new { success = false, message = "No se pudo borrar la categoria" });

            _unidadTrabajo.Marca.Remover(marcaBD);
            await _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Se borró la categoria" });
        }

        [ActionName("validarNombre")]
        public async Task<IActionResult> ValidarNombre(string nombre, int id = 0)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return Json(new { data = false });

            var nombreM = nombre.Trim().ToLower();

            var lista = await _unidadTrabajo.Marca.ObtenerTodos();

            bool existe;

            if (id == 0)
            {
                existe = lista.Any(x => (x.Nombre ?? "").Trim().ToLower() == nombreM);
            }
            else
            {
                existe = lista.Any(x =>
                            (x.Nombre ?? "").Trim().ToLower() == nombreM &&
                            x.Id == id);
            }

            return Json(new { data = existe });

        }


        #endregion
    }
}
