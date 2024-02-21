using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace negocio
{
     public class AccesoDatos
    {
        private SqlConnection conexion;
        private SqlDataReader lectura;
        private SqlCommand comando;

       public AccesoDatos()
        {
             conexion=new SqlConnection("server=.\\SQLEXPRESS;database=POKEDEX_DB;integrated security=true");//OTRA FORMA DE PASARLE DIRECTAMENTE LA DIRECCION 
             comando=new SqlCommand();

        }

        //GETER DEL LECTOR
        public SqlDataReader Lector
        {
            get {return lectura;} 
        }
        public void establecerconexion(string direccion)
        {
            conexion.ConnectionString = direccion;
        }
        public void establecerQuery(string Query)
        {
            comando.CommandType = System.Data.CommandType.Text;
            comando.CommandText = Query;

        }
        public void ejecutarQuery()
        {
            comando.Connection=conexion;
            try
            {
                conexion.Open();
                comando.ExecuteNonQuery();
                


            }
            catch(Exception ex) {
                throw ex;
            }
        }

        public void ejecutarLectura()
        {
            comando.Connection = conexion;
            try
            {
                conexion.Open();
                lectura = comando.ExecuteReader();

            }
            catch (Exception ex)
            {
                throw ex;

            }

        }

        public void cerrarConexion()
        {
            if (lectura != null) {lectura.Close(); }
            conexion.Close();
        }

        public void SetearParametros(string dat,object valor)//EL OBJ RECIBE CUALQUIER OBJ DE .NET
        {
            comando.Parameters.AddWithValue(dat, valor); 
        }

    }
}
