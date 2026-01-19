using AccesoDatos.Data;
using AccesoDatos.Repositorio.IRepositorio;
using Modelos.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccesoDatos.Repositorio
{
    public class UsuarioRepositorio : Repositorio<Usuario>, IUsuarioRepositorio
    {
        private readonly ApplicationDbContext ctx;
        public UsuarioRepositorio(ApplicationDbContext db) : base(db)
        {
            ctx = db;
        }

        
    }
}
