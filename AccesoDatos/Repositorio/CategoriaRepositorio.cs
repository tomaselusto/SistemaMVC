using AccesoDatos.Data;
using AccesoDatos.Repositorio.IRepositorio;
using Modelos.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccesoDatos.Repositorio
{
    public class CategoriaRepositorio : Repositorio<Categoria>, ICategoriaRepositorio
    {
        private readonly ApplicationDbContext ctx;
        public CategoriaRepositorio(ApplicationDbContext db) : base(db)
        {
            ctx = db;
        }

        public void Actualizar(Categoria categoria)
        {
            var categoriaBD = ctx.Categorias.FirstOrDefault(b => b.Id == categoria.Id);  //Capturo el registro actual

            if (categoriaBD != null)
            {
                //lo encontró -> lo actulizo
                categoriaBD.Nombre = categoria.Nombre;
                categoriaBD.Descripcion = categoria.Descripcion;
                categoriaBD.Estado = categoria.Estado;
                ctx.SaveChanges();

            }
        }
    
    }
}
