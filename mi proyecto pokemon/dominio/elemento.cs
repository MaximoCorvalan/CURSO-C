using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    public  class elemento
    {
        public  string _Descripcion { get; set; }
        public int  _id { get; set; }

        public override string ToString()
        {
            return _Descripcion;
        }
    }
}
