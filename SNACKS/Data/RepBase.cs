using NHibernate;
using SNACKS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using NHibernate.Linq;

namespace SNACKS.Data
{
    public class RepBase : IRepBase
    {
        private ISessionFactory factory;

        public RepBase(ISessionFactory factory)
        {
            this.factory = factory;
        }

        public async Task<bool> RegistrarAsync(object entity)
        {
            using (var sn = factory.OpenSession())
            {
                using (var tx = sn.BeginTransaction())
                {
                    await sn.SaveAsync(entity);
                    await tx.CommitAsync();
                }
            }
            return true;
        }

        public async Task<ListaRetorno<T>> ObtenerTodosAsync<T>(Paginacion paginacion, List<Expression<Func<T, bool>>> filtros, Expression<Func<T, object>> orden = null)
        {
            List<T> lista = null;
            int totalRegistros = 0;

            using (var sn = factory.OpenSession())
            {
                IQueryable<T> query = sn.Query<T>();

                foreach (var expresion in filtros)
                {
                    query = query.Where(expresion);
                }

                totalRegistros = await query.CountAsync();

                query = query.Skip((paginacion.Pagina - 1) * paginacion.Registros)
                                    .Take(paginacion.Registros);

                if (orden != null)
                {
                    query = query.OrderByDescending(orden);
                }

                lista = await query.ToListAsync();
            }

            return new ListaRetorno<T>
            {
                Lista = lista,
                TotalRegistros = totalRegistros
            };
        }
    }
}
