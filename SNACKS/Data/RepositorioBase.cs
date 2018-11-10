using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NHibernate;
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

        public void AgregarReferencia(object Referencia)
        {
            var Entidad = Context.ChangeTracker.Entries().Where(e => 
                 e.Entity.GetType().Name.Equals(Referencia.GetType().Name)
                    && (int)e.Entity.GetType().GetProperty("Id" + e.Entity.GetType().Name).GetValue(e.Entity) 
                        == (int)Referencia.GetType().GetProperty("Id" + Referencia.GetType().Name).GetValue(Referencia)).FirstOrDefault();

            if (Entidad != null)
            {
                Entidad.State = EntityState.Detached;
            }

            Context.Attach(Referencia);
        }

        public void LimpiarContexto()
        {
            var Entidades = Context.ChangeTracker.Entries().ToList();
            foreach (var Entidad in Entidades)
            {
                Entidad.State = EntityState.Detached;
            }
        }

        public void AgregarReferencias(object[] Referencias)
        {

            foreach (object Referencia in Referencias)
            {
                AgregarReferencia(Referencia);
            }
        }

        public async Task<bool> ActualizarAsync(T Entidad, bool Confirmar = true)
        {
            Context.Entry(Entidad).State = EntityState.Modified;
            if (Confirmar)
            {
                await Context.SaveChangesAsync();
            }
            return true;
        }

        public async Task<bool> EliminarAsync(T Entidad, bool Confirmar = true)
        {
            Context.Set<T>().Remove(Entidad);
            if (Confirmar)
            {
                await Context.SaveChangesAsync();
            }
            return true;
        }

        public async Task<bool> EliminarAsync(T[] Entidad, bool Confirmar = true)
        {
            Context.Set<T>().RemoveRange(Entidad);
            if (Confirmar)
            {
                await Context.SaveChangesAsync();
            }
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

        public async Task<List<T>> ObtenerTodosAsync()
        {
            return await Context.Set<T>().ToListAsync();
        }

        public async Task<List<T>> ObtenerTodosAsync(List<Expression<Func<T, bool>>> Filtros, string[] Includes = null)
        {
            IQueryable<T> Query = Context.Set<T>();

            if (Includes != null)
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

            var Lista = await Query.ToListAsync();

            return Lista;
        }

        public async Task<T> ObtenerPrimeroAsync(List<Expression<Func<T, bool>>> Filtros, string[] Includes = null)
        {
            IQueryable<T> Query = Context.Set<T>();

            if (Includes != null)
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

            var Entidad = await Query.FirstOrDefaultAsync();

            return Entidad;
        }

        public async Task<ListaRetorno<T>> ObtenerTodosAsync(Paginacion Paginacion, List<Expression<Func<T, bool>>> Filtros, string[] Includes = null, Expression<Func<T, object>> Orden = null)
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

            Query = Query.Skip((Paginacion.Pagina - 1) * Paginacion.Registros)
                                    .Take(Paginacion.Registros);

            if(Orden != null)
            {
                Query = Query.OrderByDescending(Orden);
            }

            var Lista = await Query.ToListAsync();

            var TotalRegistros = await Query.CountAsync();

            return new ListaRetorno<T>
            {
                Lista = Lista,
                TotalRegistros = TotalRegistros
            };
        }

        public async Task<bool> RegistrarAsync(T Entidad, bool Confirmar = true)
        {
            Context.Set<T>().Add(Entidad);
            if (Confirmar)
            {
                await Context.SaveChangesAsync();
            }
            return true;
        }
        
        public async Task<bool> RegistrarAsync(T[] Entidad, bool Confirmar = true)
        {
            Context.Set<T>().AddRange(Entidad);
            if (Confirmar)
            {
                await Context.SaveChangesAsync();
            }
            return true;
        }

        public async Task<List<Estadistica>> ObtenerEstadisticasAsync(object[] valores)
        {
            string sql = "Usp_Estadisticas @p0";
            for (int i = 1; i < valores.Length; i++)
            {
                sql += string.Format(", @p{0}", i);
            }
            List<Estadistica> lista = await Context.Estadistica.FromSql(sql, valores).ToListAsync();
            return lista;
        }
    }
}
