using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
     public class Articulo
    {
         public int _Id { get; set; }

        [DisplayName("Código")]
        public String  Codigo { get; set; }

        public Categoria Categoria { get; set; }
        public Marca Marca { get; set; }
        public string Nombre { get; set; }
   
        [DisplayName("Imagen")]
        public  string _imagenUrl{ get; set; }
        public decimal Precio { get; set; }
        [DisplayName("Descripción")]
        public string _Descripcion { get; set; }

    }
}
