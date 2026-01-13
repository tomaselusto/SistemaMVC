using Modelos.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccesoDatos.Repositorio.IRepositorio
{
    public interface ICategoriaRepositorio:IRepositorio<Categoria>
    {
        //el método de actualizar es individual porque cada elemento es diferente.
        void Actualizar(Categoria categoria);
    }
}
