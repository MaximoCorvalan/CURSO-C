using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dominio;

namespace negocio
{
   public class ElementoNegocio
    {
        public List<elemento> listarElemento()
        {
      
            List<elemento> elementos = new List<elemento>();   
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.establecerQuery("Select id,Descripcion from ELEMENTOS");
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {

                   elemento aux=new elemento();
                    aux._id = (int)datos.Lector["Id"];
                    aux._Descripcion = (string)datos.Lector["Descripcion"];

                    elementos.Add(aux);

                   
                }


            }catch (Exception ex)
            {


            }
            finally
            {
                datos.cerrarConexion();
            }



            return elementos;


        }

    }
}
