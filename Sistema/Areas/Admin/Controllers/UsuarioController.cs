using AccesoDatos.Data;
using AccesoDatos.Repositorio.IRepositorio;
using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Utilidades;

namespace Sistema.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = DS.RolAdmin)] //autorización
    public class UsuarioController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;
        private readonly ApplicationDbContext _db;

        public UsuarioController(IUnidadTrabajo ut, ApplicationDbContext db )
        {
            _unidadTrabajo= ut;
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }
        public class ToggleLockRequest
        {
            public string Id { get; set; }
        }
        #region API
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos() //mostrar todos los usaurios y roles.
        {
            var usuariosLista = await _unidadTrabajo.Usuario.ObtenerTodos(); //traigo usuarios
            var usuarioRolLista = await _db.UserRoles.ToListAsync(); //traigo roles y usuarios
            var roles = await _db.Roles.ToListAsync(); //traigo roles

            foreach (var usr in usuariosLista)
            {
                var roleId = usuarioRolLista.FirstOrDefault(u => u.UserId == usr.Id).RoleId;
                usr.Rol = roles.FirstOrDefault(u => u.Id == roleId).Name;
            }

            return Json(new { data = usuariosLista });
        }

        [HttpPost]
        public async Task<IActionResult> BloquearDesbloquear([FromBody] ToggleLockRequest req)
        {
            if (req == null || string.IsNullOrWhiteSpace(req.Id))
                return Json(new { success = false, message = "Id inválido" });

            var usr = await _unidadTrabajo.Usuario.ObtenerPrimero(u => u.Id == req.Id);
            if (usr == null)
                return Json(new { success = false, message = "Error de usuario" });

            if (usr.LockoutEnd != null && usr.LockoutEnd > DateTime.Now)
                usr.LockoutEnd = DateTime.Now;            // desbloquear
            else
                usr.LockoutEnd = DateTime.Now.AddYears(1000); // bloquear

            await _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Operación exitosa" });
        }

        #endregion
    }
}
