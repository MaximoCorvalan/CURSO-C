using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using dominio;

namespace negocio
{
    public class pokemondata
    {
        private AccesoDatos datos = new AccesoDatos();
        public List<pokemon> listarPokemon()
        {
       
           
            List<pokemon>listaPokemones = new List<pokemon>();
            
            try
            {

                datos.establecerconexion("server=.\\SQLEXPRESS;database=POKEDEX_DB;integrated security=true");
                datos.establecerQuery("select Nombre,Numero ,p.Descripcion ,UrlImagen,e.Descripcion tipo, D.descripcion debilidad,D.Id as idDebilidad,e.Id as idtipo,p.Id,p.Activo from POKEMONS p, ELEMENTOS e, ELEMENTOS D where e.Id=p.IdTipo and d.Id=p.IdDebilidad ;");
                datos.ejecutarLectura();

               //FORMA SIN CLASE:
              /* conexion.ConnectionString = "server=.\\SQLEXPRESS;database=POKEDEX_DB;integrated security=true";
                comando.CommandType = System.Data.CommandType.Text;
                comando.CommandText = "select Nombre ,p.Descripcion ,UrlImagen,e.Descripcion tipo, D.descripcion debilidad from POKEMONS p, ELEMENTOS e, ELEMENTOS D where e.Id=p.Id and d.Id=p.IdDebilidad ;";
                comando.Connection = conexion;
                conexion.Open();
                lectura=comando.ExecuteReader();*/

           
                while(datos.Lector.Read())
                {

                    pokemon aux = new pokemon();
                    aux._descripcion = (string)datos.Lector["Descripcion"];
                    aux.debilidad = new elemento(); //CREO INSTANCIA Y SE LO ASIGNO A DEBILIDAD
                    aux.tipo = new elemento();
                    aux._id = (int)datos.Lector["Id"];
                    aux._numero = (int)datos.Lector["Numero"];
                    aux.debilidad._id = (int)datos.Lector["idDebilidad"];
                    aux.debilidad._Descripcion = (string)datos.Lector["debilidad"];
                    aux.tipo._id = (int)datos.Lector["idtipo"];
                    aux.tipo._Descripcion = (string)datos.Lector["tipo"];// PODES CREAR UNA INSTANCIA DE ELEMNTO ARRIBA Y TE AHORRAS HACERLO DENTRO DE LA CLASE.
                    aux._nombre = (string)datos.Lector["Nombre"];
                    aux._urlImagen = (string)datos.Lector["UrlImagen"];
                    aux.activo = (bool)datos.Lector["Activo"];

                    if (aux.activo == true)//ELIMINACION LOGICA 
                    {  
                        listaPokemones.Add(aux);
                    }

                   



                }
                Console.WriteLine(listaPokemones.Count + "NUMEROOO");
                return listaPokemones;


            }
            catch (Exception ex)
            {
                
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
              


            }

           



        }

       


        public void AgregarPokemon(pokemon pokemon)
        {
            

            try
            {
                
                
               datos.establecerQuery("insert into POKEMONS(IdDebilidad, IdTipo, UrlImagen, Nombre,Descripcion,Numero, Activo) values(@idDebilidad,@idTipo,'"+pokemon._urlImagen+"', '"+pokemon._nombre+"','"+pokemon._descripcion+"',"+pokemon._numero+", 1)");
                //EL SETEAR PARAMETROS ES UN METODO , EL CUAL REALIZA LA FUNCION DE PARAMETROS POR COMANDO EN
                //DONDE ASIGNAMOS EN ESTABLECERQUERY UNA VARIABLE Y MEDIANTE ESTE METODO UN VALOR.
                //SE ESTABLECE LA VARIABLE CON EL @
                datos.SetearParametros("@idTipo", pokemon.tipo._id);
                datos.SetearParametros("@idDebilidad", pokemon.debilidad._id);

                datos.ejecutarQuery();
            

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {  
                datos.cerrarConexion();

            }


            



        }
        public void bajafisica(int id)
        {
            try
            {
                datos.establecerQuery("delete from pokemons where id=" + id + "");
                datos.ejecutarQuery();
                datos.cerrarConexion();


            }catch (Exception ex)
            {

                throw ex;
            }


        }

        public void bajaLogica(int id)
        {
            try
            {
                datos.establecerQuery("update pokemons set Activo=0 where id=" + id + "");
                datos.ejecutarQuery();
                datos.cerrarConexion();

            }
            catch (Exception ex) { 

                throw ex;
            
            
            }
           


        }

        public void modificarPokemon(pokemon pokemon)
        {
            try
            {
                

                datos.establecerQuery("update  POKEMONS set Descripcion ='" + pokemon._descripcion + "', Numero=" + pokemon._numero + ",Nombre='" + pokemon._nombre + "',UrlImagen='" + pokemon._urlImagen + "',IdTipo=" + pokemon.tipo._id + ",IdDebilidad=" + pokemon.debilidad._id + " where Id="+pokemon._id+"");
                datos.ejecutarQuery();
                
            }catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
           
        }
    }
}
