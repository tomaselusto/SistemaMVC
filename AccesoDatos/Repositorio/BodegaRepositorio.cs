using AccesoDatos.Data;
using AccesoDatos.Repositorio.IRepositorio;
using Modelos.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccesoDatos.Repositorio
{
    public class BodegaRepositorio : Repositorio<Bodega>, IBodegaRepositorio
    {
        private readonly ApplicationDbContext ctx;
        public BodegaRepositorio(ApplicationDbContext db) : base(db)
        {
            ctx = db;
        }

        public void Actualizar(Bodega bodega)
        {
            var bodegaBD = ctx.Bodegas.FirstOrDefault(b => b.Id == bodega.Id);  //Capturo el registro actual

            if (bodegaBD != null)
            {
                //lo encontró -> lo actulizo
                bodegaBD.Nombre= bodega.Nombre;
                bodegaBD.Descripcion= bodega.Descripcion;
                bodegaBD.Estado= bodega.Estado;
                ctx.SaveChanges();

            }
        }
    }
}
