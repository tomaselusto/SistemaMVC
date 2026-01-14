using Modelos.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccesoDatos.Repositorio.IRepositorio
{
    public interface IMarcaRepositorio:IRepositorio<Marca>
    {
        //el método de actualizar es individual porque cada elemento es diferente.
        void Actualizar(Marca Marca);
    }
}
