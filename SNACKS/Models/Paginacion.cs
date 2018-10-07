using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SNACKS.Models
{
    public class Paginacion
    {
        public int Registros { get; set; }
        public int Pagina { get; set; }
        public List<Filtro> Filtros { get; set; }
    }

    public class Filtro
    {
        public int K { get; set; }
        public string V { get; set; }
        public int N { get; set; }
        public DateTime D { get; set; }
    }

    public class ListaRetorno<T>
    {
        public List<T> Lista { get; set; }
        public int TotalRegistros { get; set; }
    }
}
