using AccesoDatos.Data;
using AccesoDatos.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Mvc.Rendering;
using Modelos.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace AccesoDatos.Repositorio
{
    public class ProductoRepositorio : Repositorio<Producto>, IProductoRepositorio
    {
        private readonly ApplicationDbContext ctx;
        public ProductoRepositorio(ApplicationDbContext db) : base(db)
        {
            ctx = db;
        }

        public void Actualizar(Producto producto)
        {
            var prodBD= ctx.Productos.FirstOrDefault(b => b.Id == producto.Id);  //Capturo el registro actual

            if (prodBD != null)
            {
                //lo encontró -> lo actulizo
                //valido la imagen
                if(producto.ImagenUrl!=null)
                    prodBD.ImagenUrl = producto.ImagenUrl;

                prodBD.Precio= producto.Precio;
                prodBD.Descripcion= producto.Descripcion;
                prodBD.Estado= producto.Estado;

                prodBD.Costo = producto.Costo;
                prodBD.NumeroSerie = producto.NumeroSerie;
                prodBD.MarcaId = producto.MarcaId;
                prodBD.CateogriaId = producto.CateogriaId;
                prodBD.PadreId = producto.PadreId;
                ctx.SaveChanges();

            }
        }

        public IEnumerable<SelectListItem> ObtenerTodosDropDownList(string obj)
        {
            if(obj=="Categoria" )
            {
                return ctx.Categorias.Where(c => c.Estado == true).Select(c => new SelectListItem
                {
                    Text = c.Nombre,
                    Value = c.Id.ToString()
                });
            }
            if (obj == "Marca")
            {
                return ctx.Marcas.Where(c => c.Estado == true).Select(c => new SelectListItem
                {
                    Text = c.Nombre,
                    Value = c.Id.ToString()
                });
            }
            return null;
        }
    }
}
