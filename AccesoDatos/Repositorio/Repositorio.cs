using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using AccesoDatos.Data;
using AccesoDatos.Repositorio.IRepositorio;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace AccesoDatos.Repositorio
{
    public class Repositorio<T> : IRepositorio<T> where T : class  //repositorio genérico
    {

        //vamos a necesitar el dbContext
        private readonly ApplicationDbContext _db;

        internal DbSet<T> dbSet;

        public Repositorio(ApplicationDbContext db)
        {
                _db=db;
                this.dbSet= _db.Set<T>();   
        }
        //usamos todo EF para trabajar con la BD.
        public  async Task Agregar(T entidad)
        {
            await dbSet.AddAsync(entidad); //equivalente a insert into table
        }

        public async Task<T> Obtener(int id) //agrego el async para indicar que es asincrono
        {
            return await dbSet.FindAsync(id); //equivalente a select * from (solo por id)
        }

        public async Task<T> ObtenerPrimero(Expression<Func<T, bool>> filtro = null, string incluirPropiedades = null, bool isTracking = true)
        {
            IQueryable<T> query = dbSet;
            if (filtro != null)
            {
                //filtramos nuestro query/consulta
                query = query.Where(filtro); //select * from where - FILTRO - 
            }
            if (incluirPropiedades != null)
            {
                //verifico si la cadena de caracteres manda valor (para los include)
                foreach (var incluirProp in incluirPropiedades.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))// lo trasnformo en char, hago un split para que lo separe por comas, además remueve los espacios vacios.
                {
                    //include nos va a servir (propio de EF) nos va a incluir las propiedades de los objetos relacionados (cuando mandamos un producto nos va a traer categorioa y marca por ej)
                    query.Include(incluirProp);
                }
            }

            if (!isTracking)
            {
                //si es false 
                query = query.AsNoTracking();
            }

            return await query.FirstOrDefaultAsync();        
        }

        public async Task<IEnumerable<T>> ObtenerTodos(Expression<Func<T, bool>> filtro = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string incluirPropiedades = null, bool isTracking = true)
        {
            IQueryable<T> query = dbSet;
            if(filtro!=null)
            {
                //filtramos nuestro query/consulta
                query= query.Where(filtro); //select * from where - FILTRO - 
            }
            if(incluirPropiedades!=null)
            {
                //verifico si la cadena de caracteres manda valor (para los include)
                foreach (var incluirProp in incluirPropiedades.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))// lo trasnformo en char, hago un split para que lo separe por comas, además remueve los espacios vacios.
                {
                    //include nos va a servir (propio de EF) nos va a incluir las propiedades de los objetos relacionados (cuando mandamos un producto nos va a traer categorioa y marca por ej)
                   query= query.Include(incluirProp);
                }
            }
            if(orderBy!=null)
                query=orderBy(query); //para dar un criterio de orden

            if(!isTracking)
            {
                //si es false 
                query= query.AsNoTracking();
            }

            return await query.ToListAsync(); //me devuelve la lista de lo que le envío
        }

        public void Remover(T entidad)
        {
            dbSet.Remove(entidad);
        }

        public void RemoverRango(IEnumerable<T> entidad)
        {
            dbSet.RemoveRange(entidad);
        }
    }
}
