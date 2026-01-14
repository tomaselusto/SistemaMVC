using Microsoft.AspNetCore.Mvc.Rendering;
using Modelos.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccesoDatos.Repositorio.IRepositorio
{
    public interface IProductoRepositorio :IRepositorio<Producto>
    {
        //el método de actualizar es individual porque cada elemento es diferente.
        void Actualizar(Producto producto);

        IEnumerable<SelectListItem> ObtenerTodosDropDownList(string obj); //el obj será categoria o producto 
    }
}
