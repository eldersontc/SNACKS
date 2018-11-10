using SNACKS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SNACKS.Data
{
    public interface IRepBase
    {
        Task<bool> RegistrarAsync(object entity);
        Task<ListaRetorno<T>> ObtenerTodosAsync<T>(Paginacion paginacion, List<Expression<Func<T, bool>>> filtros, Expression<Func<T, object>> orden = null);
    }
}
