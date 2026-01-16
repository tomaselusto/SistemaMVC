using Modelos.Especificaciones;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace AccesoDatos.Repositorio.IRepositorio
{
    public interface IRepositorio<T> where T : class //donde T es una clase
    {
        //Task <T>  para que sea asíncrono
        Task<T> Obtener(int id);

        Task<IEnumerable<T>> ObtenerTodos(
            Expression<Func<T, bool>> filtro =null, //hará los filtros
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy =null,  //esto es para ordenarlos
            string incluirPropiedades=null, 
            bool isTracking =true //cuando queramos acceder a un objeto o lista y lo queramos modificar (FALSE)
            );

        Task<T> ObtenerPrimero(
            Expression<Func<T, bool>> filtro = null, //hará los filtros            
            string incluirPropiedades = null,
            bool isTracking = true //cuando queramos acceder a un objeto o lista y lo queramos modificar (FALSE)
             );

        Task Agregar(T entidad);
        void Remover (T entidad); // no pueden ser asíncronos
        void RemoverRango (IEnumerable<T> entidad);// no pueden ser asíncronos


        PagedList<T> ObtenerTodosPaginados(Parametros parametros, Expression<Func<T, bool>> filtro = null, //hará los filtros
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,  //esto es para ordenarlos
            string incluirPropiedades = null,
            bool isTracking = true);
    }
}
