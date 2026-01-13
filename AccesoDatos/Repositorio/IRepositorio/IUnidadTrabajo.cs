using System;
using System.Collections.Generic;
using System.Text;

namespace AccesoDatos.Repositorio.IRepositorio
{
    public interface IUnidadTrabajo :IDisposable //Deshacerte de cualquier recurso que no utilices
    {
        //envolvemos los respositorios que tenemos
        IBodegaRepositorio Bodega { get;  }
        ICategoriaRepositorio Categoria { get; }
        Task Guardar(); //guardara con el savechanges()

    }
}
