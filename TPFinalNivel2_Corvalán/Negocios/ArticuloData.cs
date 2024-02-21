using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;

namespace Negocios
{
    public class ArticuloData
    {
        private AccesoDatos accesoDatos=new AccesoDatos();
     

        public List<Articulo> listarArticulos()
        {
               List<Articulo> listarArt = new List<Articulo>();
            accesoDatos.SetearQuery("select art.id, art.Codigo,art.Nombre ,art.Descripcion,art.Precio,art.ImagenUrl,mrc.Descripcion as marca,cat.Descripcion as categoria,mrc.id as mrcid,cat.id as catid from ARTICULOS art,MARCAS mrc, CATEGORIAS cat where art.IdCategoria=cat.Id and art.IdMarca=mrc.Id ");
            accesoDatos.ejecutarLectura();

            try
            {  //TENGO QUE VALIDAR EL DBNULL

                while (accesoDatos.lector.Read() != false)
                {
                    Articulo aux = new Articulo();
                    aux._Id = (int)accesoDatos.lector["Id"];
                    aux.Marca=new Marca();
                    aux.Categoria=new Categoria(); 
                    aux.Codigo = (string)accesoDatos.lector["Codigo"];
                    aux._Descripcion = (string)accesoDatos.lector["Descripcion"];
                      aux.Precio = (decimal)accesoDatos.lector["Precio"];
                  string precioFormateado = aux.Precio.ToString("#,####.00##");
                  aux.Precio = decimal.Parse( precioFormateado);
                    aux.Marca._id = (int)accesoDatos.lector["mrcid"];
                    aux.Categoria._Id = (int)accesoDatos.lector["catid"];
                    aux.Categoria._Descripcion = (string)accesoDatos.lector["categoria"];
                    aux.Marca._Descripcion = (string)accesoDatos.lector["marca"];
                    aux.Nombre = (string)accesoDatos.lector["Nombre"];
                    if (!(accesoDatos.lector["ImagenUrl"] is DBNull))
                    aux._imagenUrl = (string)accesoDatos.lector["ImagenUrl"];




                    listarArt.Add(aux);



                }
             
                return listarArt;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                 accesoDatos.cerrarConexion();
            }


        }

        public void EliminarArticulo(int id)
        {
            try
            {
                accesoDatos.SetearQuery("delete from Articulos where id="+id+"");
            
                accesoDatos.ejecutar();
               
            }catch (Exception ex) { throw ex; }
            finally
            {
                accesoDatos.cerrarConexion();
            }
        }

        public void agregarArticulo(Articulo art)
        {
            try
            {  //SI LE PASO TODOS ASI POR PARAMENTRO  ME PUEDO LLEGAR A AHORRAR VALIDACION DE SI ESTA VACIO EN ALGUNOS CASOS/ NO ME DEJA MANDARLE NULL SINO TIENE NADA POR PARAMETROS DEL COMANDO
                accesoDatos.SetearQuery("insert into ARTICULOS(Codigo,Nombre,Descripcion,IdMarca,IdCategoria,ImagenUrl,Precio) values (@codigo,@nombre,@descripcion,@idMarca,@idCat,'"+art._imagenUrl+"',@precio)");
                accesoDatos.setearParametro("@nombre", art.Nombre);
                accesoDatos.setearParametro("@codigo", art.Codigo);
                accesoDatos.setearParametro("@descripcion", art._Descripcion);
                accesoDatos.setearParametro("@precio", art.Precio);
               // AccesoDatos.setearParametro("@imagen", art._imagenUrl);
                accesoDatos.setearParametro("@idMarca", art.Marca._id);
                accesoDatos.setearParametro("@idCat", art.Categoria._Id);
                accesoDatos.ejecutar();

                
            }catch(Exception ex) { 
                throw ex; 
            }
            finally { 
               accesoDatos.cerrarConexion();
            }
        }

        public void modificarArticulo(Articulo art)
        {
            try
            {
                accesoDatos.SetearQuery("update  ARTICULOS set Codigo=@codigo,Nombre=@nombre,Descripcion=@descripcion,IdMarca=@idMarca,IdCategoria=@idCat,ImagenUrl='" + art._imagenUrl + "',Precio=@precio where id="+art._Id+"");
                accesoDatos.setearParametro("@nombre", art.Nombre);
                accesoDatos.setearParametro("@codigo", art.Codigo);
                accesoDatos.setearParametro("@descripcion", art._Descripcion);
                accesoDatos.setearParametro("@precio", art.Precio);
                // AccesoDatos.setearParametro("@imagen", art._imagenUrl);
                accesoDatos.setearParametro("@idMarca", art.Marca._id);
                accesoDatos.setearParametro("@idCat", art.Categoria._Id);
                accesoDatos.ejecutar();
            }catch (Exception ex) { throw ex; }
            finally { accesoDatos.cerrarConexion(); }

        }

        public List<Articulo> BuscadaFiltrada(string campo,string criterio, string filtro) {

            List<Articulo> listaAux=new List<Articulo>();

            string query = "select art.id, art.Codigo,art.Nombre ,art.Descripcion,art.Precio,art.ImagenUrl,mrc.Descripcion as marca,cat.Descripcion as categoria,mrc.id as mrcid,cat.id as catid from ARTICULOS art,MARCAS mrc, CATEGORIAS cat where art.IdCategoria=cat.Id and art.IdMarca=mrc.Id";
            try
            {
                switch (campo)
            {

                case "PRECIO":

                    switch (criterio)
                    {
                        case "MAYOR A":

                            query+=" AND art.Precio > "+filtro+"";

                         break;

                        case "MENOR A":
                            query += " AND art.Precio < " + filtro + "";

                            break;

                        case "IGUAL A":
                            query += " AND art.Precio = " + filtro + "";

                            break;

                    }

                 break;

                case "NOMBRE":
               

                    switch (criterio)
                    {
                        case "COMIENZA CON":

                            query += " and art.nombre like '"+filtro+"%' ";

                        break;

                        case "TERMINA CON":
                            query += " and art.nombre like '%" + filtro + "' ";

                         break;

                        case "CONTIENE":

                            query += " and art.nombre like '%" + filtro + "%' ";
                         break;

                    }

                break;

                case "CODIGO":

                    switch (criterio)
                    {
                        case "COMIENZA CON":

                            query += " and art.codigo like '" + filtro + "%' ";

                            break;

                        case "TERMINA CON":
                            query += " and art.codigo like '%" + filtro + "' ";

                            break;

                        case "CONTIENE":

                            query += " and art.codigo like '%" + filtro + "%' ";
                            break;

                    }
                 break;

                    case "CATEGORIA":
                       

                        query += " and cat.Descripcion = '"+criterio+"'";

                    break;

                    case "MARCA":
                        query += " and mrc.Descripcion = '"+criterio+"'";
                    break;

                  


            }

           
              //TENGO QUE VALIDAR EL DBNULL
                accesoDatos.SetearQuery(query);
                accesoDatos.ejecutarLectura();

                while (accesoDatos.lector.Read() != false)
                {
                    Articulo aux = new Articulo();
                    aux._Id = (int)accesoDatos.lector["Id"];
                    aux.Marca = new Marca();
                    aux.Categoria = new Categoria();
                    aux.Codigo = (string)accesoDatos.lector["Codigo"];
                    aux._Descripcion = (string)accesoDatos.lector["Descripcion"];
                    aux.Precio = (decimal)accesoDatos.lector["Precio"];
                    string precioFormateado = aux.Precio.ToString("#,####.00##");
                    aux.Precio = decimal.Parse(precioFormateado);
                    aux.Marca._id = (int)accesoDatos.lector["mrcid"];
                    aux.Categoria._Id = (int)accesoDatos.lector["catid"];
                    aux.Categoria._Descripcion = (string)accesoDatos.lector["categoria"];
                    aux.Marca._Descripcion = (string)accesoDatos.lector["marca"];
                    aux.Nombre = (string)accesoDatos.lector["Nombre"];
                    if (!(accesoDatos.lector["ImagenUrl"] is DBNull))
                        aux._imagenUrl = (string)accesoDatos.lector["ImagenUrl"];




                    listaAux.Add(aux);



                }

                return listaAux;

            }
            catch (Exception ex)
            {
               
              
                throw ex;
            }
            finally
            {
                accesoDatos.cerrarConexion();
            }
  
        
        }

        
       
    }

  }

