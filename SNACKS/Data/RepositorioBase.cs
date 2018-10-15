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

        public async Task<bool> ActualizarAsync(T Entidad, object[] Referencias = null)
        {
            if (Referencias != null)
            {
                foreach (object Referencia in Referencias)
                {
                    Context.Entry(Referencia).State = EntityState.Unchanged;
                }
            }
            Context.Entry(Entidad).State = EntityState.Modified;
            await Context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EliminarAsync(T[] Entidad)
        {
            Context.Set<T>().RemoveRange(Entidad);
            await Context.SaveChangesAsync();
            return true;
        }

        public async Task<T> ObtenerAsync(int Id, string[] Includes = null)
        {
            var Key = Context.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties[0];

            IQueryable<T> Query = Context.Set<T>();

            if (Includes != null)
            {
                foreach (var Include in Includes)
                {
                    Query = Query.Include(Include);
                }
            }

            return await Query.FirstOrDefaultAsync(e => EF.Property<int>(e, Key.Name) == Id);
        }

        public async Task<ListaRetorno<T>> ObtenerTodosAsync(Paginacion Paginacion, List<Expression<Func<T, bool>>> Filtros, string[] Includes = null)
        {
            IQueryable<T> Query = Context.Set<T>();

            if(Includes != null)
            {
                foreach (var Include in Includes)
                {
                    Query = Query.Include(Include);
                }
            }

            foreach (var Expresion in Filtros)
            {
                Query = Query.Where(Expresion);
            }

            var Lista = await Query.Skip((Paginacion.Pagina - 1) * Paginacion.Registros)
                                    .Take(Paginacion.Registros)
                                    .ToListAsync();

            var TotalRegistros = await Query.CountAsync();

            return new ListaRetorno<T>
            {
                Lista = Lista,
                TotalRegistros = TotalRegistros
            };
        }

        public async Task<bool> RegistrarAsync(T Entidad, object[] Referencias = null)
        {
            if(Referencias != null)
            {
                foreach (object Referencia in Referencias)
                {
                    Context.Entry(Referencia).State = EntityState.Unchanged;
                }
            }
            Context.Set<T>().Add(Entidad);
            await Context.SaveChangesAsync();
            return true;
        }
    }
}
