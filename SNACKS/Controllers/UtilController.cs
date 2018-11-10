using Microsoft.AspNetCore.Mvc;
using NHibernate;
using SNACKS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SNACKS.Controllers
{
    public class UtilController : ControllerBase
    {
        public ISessionFactory factory;

        public UtilController(ISessionFactory factory)
        {
            this.factory = factory;
        }

        public void AsignarPaginacion<T>(Paginacion paginacion, ref IQueryable<T> query)
        {
            query = query.Skip((paginacion.Pagina - 1) * paginacion.Registros)
                                .Take(paginacion.Registros);
        }
    }
}
