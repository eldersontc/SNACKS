using SNACKS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SNACKS.Data
{
    public interface IRepositorioBase<T> where T : class
    {
        Task<List<T>> ObtenerTodosAsync(List<Expression<Func<T, bool>>> Filtros, string[] Includes = null);
        Task<ListaRetorno<T>> ObtenerTodosAsync(Paginacion Paginacion, List<Expression<Func<T, bool>>> Filtros, string[] Includes = null);
        Task<T> ObtenerAsync(int Id, string[] Includes = null);
        Task<bool> RegistrarAsync(T Entidad, object[] Referencias = null);
        Task<bool> ActualizarAsync(T Entidad, object[] Referencias = null);
        Task<bool> EliminarAsync(T[] Entidad);
    }
}
