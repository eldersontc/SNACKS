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
        Task<ListaRetorno<T>> ObtenerTodosAsync(Paginacion Paginacion, List<Expression<Func<T, bool>>> Filtros, List<string> Includes = null);
        Task<T> ObtenerAsync(int Id, List<string> Includes = null);
        Task<bool> RegistrarAsync(T Entidad);
        Task<bool> ActualizarAsync(T Entidad);
        Task<bool> EliminarAsync(T Entidad);
    }
}
