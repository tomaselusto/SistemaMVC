using Modelos.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccesoDatos.Repositorio.IRepositorio
{
    public interface IBodegaRepositorio :IRepositorio<Bodega>
    {
        //el método de actualizar es individual porque cada elemento es diferente.
        void Actualizar(Bodega bodega);

    }
}
