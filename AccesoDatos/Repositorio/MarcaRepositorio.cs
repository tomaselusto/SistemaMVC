using AccesoDatos.Data;
using AccesoDatos.Repositorio.IRepositorio;
using Modelos.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccesoDatos.Repositorio
{
    public class MarcaRepositorio : Repositorio<Marca>, IMarcaRepositorio
    {
        private readonly ApplicationDbContext ctx;
        public MarcaRepositorio(ApplicationDbContext db) : base(db)
        {
            ctx = db;
        }

        public void Actualizar(Marca marca)
        {
            var marcaBD = ctx.Marcas.FirstOrDefault(b => b.Id == marca.Id);  //Capturo el registro actual

            if (marcaBD != null)
            {
                //lo encontró -> lo actulizo
                marcaBD.Nombre = marca.Nombre;
                marcaBD.Descripcion = marca.Descripcion;
                marcaBD.Estado = marca.Estado;
                ctx.SaveChanges();

            }
        }
    }
}
