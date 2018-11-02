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
        void AgregarReferencia(object Referencia);
        void AgregarReferencias(object[] Referencias);
        Task<List<T>> ObtenerTodosAsync(List<Expression<Func<T, bool>>> Filtros, string[] Includes = null);
        Task<ListaRetorno<T>> ObtenerTodosAsync(Paginacion Paginacion, List<Expression<Func<T, bool>>> Filtros, string[] Includes = null, Expression<Func<T, object>> Orden = null);
        Task<T> ObtenerAsync(int Id, string[] Includes = null);
        Task<bool> RegistrarAsync(T Entidad, bool Confirmar = true);
        Task<bool> RegistrarAsync(T[] Entidad, bool Confirmar = true);
        Task<bool> ActualizarAsync(T Entidad, bool Confirmar = true);
        Task<bool> EliminarAsync(T Entidad, bool Confirmar = true);
        Task<bool> EliminarAsync(T[] Entidad, bool Confirmar = true);
        Task<List<Estadistica>> ObtenerEstadisticasAsync(object[] valores);
    }
}
