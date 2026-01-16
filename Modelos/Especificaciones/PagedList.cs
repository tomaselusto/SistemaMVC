using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Modelos.Especificaciones
{
    public class PagedList<T> : List<T>
    {
        public MetaData MetaData { get; set; }
        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            MetaData = new MetaData
            {
                TotalCount = count,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(count / (double)pageSize) //1.5 lo transforma a 2

            };
            AddRange(items); // agrega los elementos de la coleccion al finalde la lista
        }


        public static PagedList<T> ToPagedList(IEnumerable<T> entidad, int pageNumer, int pageSize)
        {
            var count = entidad.Count();
            var items = entidad.Skip((pageNumer - 1) * pageSize).Take(pageSize).ToList();
            return new PagedList<T>(items, count, pageNumer, pageSize);

            //método para la paginación
        }
    }

}
