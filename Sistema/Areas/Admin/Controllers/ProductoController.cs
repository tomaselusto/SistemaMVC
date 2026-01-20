using AccesoDatos.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modelos.Models;
using Modelos.ViewModels;
using Utilidades;

namespace Sistema.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = DS.RolAdmin +"," + DS.RolInventario)]
    public class ProductoController : Controller
    {
        //llamamos a nuestra unidad de trabajo
        private readonly IUnidadTrabajo _unidadTrabajo;
        //manejar acceso a recrusos estaticos tipo rutas de imagenes
        private readonly IWebHostEnvironment _webHostEnviorment;

        public ProductoController(IUnidadTrabajo ut, IWebHostEnvironment whe )
        {
             _unidadTrabajo= ut;
            _webHostEnviorment= whe;
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
                MarcaLista=_unidadTrabajo.Producto.ObtenerTodosDropDownList("Marca"),
                PadreLista = _unidadTrabajo.Producto.ObtenerTodosDropDownList("Producto")
            };
            if(id==null)
            {
                //crear nuevo producto
                prodVM.Producto.Estado = true;
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

        [HttpPost]
        public async Task<IActionResult> Upsert (ProductoVM prodVM) //recibirá el view model
        {
            if(ModelState.IsValid)
            {
                var files= HttpContext.Request.Form.Files; //valida todos los archivos que le pasamos por post
                string webRootPath = _webHostEnviorment.WebRootPath; //ruta donde se grabará nuestra imagen

                if(prodVM.Producto.Id==0)
                {
                    //nuevo producto
                    string upload = webRootPath + DS.ImagenRuta;
                    string fileName= Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);
                    using(var fileStream = new FileStream(Path.Combine(upload,fileName +extension),FileMode.Create)) 
                    {
                        files[0].CopyTo(fileStream); //copio la imagen física en un espacio en memoria
                    }
                    prodVM.Producto.ImagenUrl = fileName + extension;
                    await _unidadTrabajo.Producto.Agregar(prodVM.Producto);


                }
                else
                {
                    //Actualizar-> si envío una nueva imagen debo reemplazar la anterior
                    var objProd= await _unidadTrabajo.Producto.ObtenerPrimero(p=>p.Id ==prodVM.Producto.Id, isTracking:false);
                    if(files.Count>0) //si se carga uan nueva imagen
                    {
                        string upload = webRootPath + DS.ImagenRuta;
                        string fileName = Guid.NewGuid().ToString(); //id unico para eso el guid
                        string extension = Path.GetExtension(files[0].FileName);
                        //borrar la img anterior
                        var anterior= Path.Combine(upload,objProd.ImagenUrl);
                        if(System.IO.File.Exists(anterior))
                        {
                            System.IO.File.Delete(anterior);
                        }
                        //Creamos la nueva img.
                        using (var fileStream = new FileStream(Path.Combine(upload, fileName+ extension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream); //copio la imagen física en un espacio en memoria
                        }
                        prodVM.Producto.ImagenUrl=fileName + extension;
                        
                    }
                    //sino se carga una nueva imagen
                    else
                    {
                        prodVM.Producto.ImagenUrl = objProd.ImagenUrl;
                    }
                    _unidadTrabajo.Producto.Actualizar(prodVM.Producto);
                }
                TempData[DS.Exitosa] = "El producto se guardó con éxito";
                await _unidadTrabajo.Guardar();
                return View("Index");
            }
            //si el modelo no es válido, cargo de nuevo las listas de categorias y marcas
            prodVM.CategoriaLista = _unidadTrabajo.Producto.ObtenerTodosDropDownList("Categoria");
            prodVM.MarcaLista = _unidadTrabajo.Producto.ObtenerTodosDropDownList("Marca");
            prodVM.PadreLista = _unidadTrabajo.Producto.ObtenerTodosDropDownList("Producto");
            return View(prodVM);
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

            //eliminar la imagen antes de eliminar el producto
            string upload = _webHostEnviorment.WebRootPath + DS.ImagenRuta;
            var imgABorrar = Path.Combine(upload, prodBD.ImagenUrl);
            if(System.IO.File.Exists(imgABorrar))
            {
                System.IO.File.Delete(imgABorrar);
            }


            //borramos al producto                
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
