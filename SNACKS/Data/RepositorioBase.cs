using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SNACKS.Models;

namespace SNACKS.Data
{
    public class RepositorioBase<T> : IRepositorioBase<T> where T : class
    {
        private readonly ApplicationDbContext Context;

        public RepositorioBase(ApplicationDbContext Context)
        {
            this.Context = Context;
        }

        public async Task<bool> ActualizarAsync(T Entidad)
        {
            Context.Entry(Entidad).State = EntityState.Modified;
            await Context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EliminarAsync(T Entidad)
        {
            Context.Set<T>().Remove(Entidad);
            await Context.SaveChangesAsync();
            return true;
        }

        public async Task<T> ObtenerAsync(int Id)
        {
            return await Context.Set<T>().FindAsync(Id);
        }

        public async Task<ListaRetorno<T>> ObtenerTodosAsync(Paginacion Paginacion, List<Expression<Func<T, bool>>> Filtros)
        {
            IQueryable<T> Query = Context.Set<T>();

            foreach (var Expresion in Filtros)
            {
                Query = Query.Where(Expresion);
            }

            var Lista = await Query.Skip((Paginacion.Pagina - 1) * Paginacion.Registros)
                                    .Take(Paginacion.Registros)
                                    .ToListAsync();

            return new ListaRetorno<T>
            {
                Lista = Lista,
                TotalRegistros = Query.Count()
            };
        }

        public async Task<bool> RegistrarAsync(T Entidad)
        {
            Context.Set<T>().Add(Entidad);
            await Context.SaveChangesAsync();
            return true;
        }
    }
}
