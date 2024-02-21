using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using dominio;
using negocio;

namespace practica1bdLECTURA
{
    public partial class main : Form
    {
        private List<pokemon> listasPokemos;
        private int contadorFiltroAvanzado = 0;
        public main()
        {
            InitializeComponent();
        }

        private void main_Load(object sender, EventArgs e)
        {


            cboCampo.Items.Add("Número");
            cboCampo.Items.Add("Nombre");
            cboCampo.Items.Add("Descripcion");

         


            cargar();


        }
        private void cargar()//CREAMOS ESTE METODO PARA QUE SE ACTUALICE LA TABLA CADA VEZ QUE
                            
        {

            pokemondata pokemondata = new pokemondata();
            try
            {
                  listasPokemos = pokemondata.listarPokemon();
                  datos.DataSource = listasPokemos;
                ocultarColumnas();



            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }
        private void ocultarColumnas()
        {
            datos.Columns[1].Visible = false; //MOSTRAMOS O NO UNA COLUMNA
            datos.Columns[5].Visible = false;


        }

        private void datos_SelectionChanged(object sender, EventArgs e)
        {
            if (datos.CurrentRow != null)
            {
                pokemon aux=(pokemon)datos.CurrentRow.DataBoundItem;
           

                 
                try
                {
                    imagen.Load(aux._urlImagen);
                }
                catch (Exception ex)
                {

                    imagen.Load("https://editorial.unc.edu.ar/wp-content/uploads/sites/33/2022/09/placeholder.png");

                }
            }

              
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            altaPokemon altaPokemon = new altaPokemon();
            altaPokemon.ShowDialog();
            cargar();
        }

        private void modificarbtn_Click(object sender, EventArgs e)
        {     pokemon pok=(pokemon)datos.CurrentRow.DataBoundItem;
             altaPokemon modificarPokemon = new altaPokemon(pok);
             
             modificarPokemon.ShowDialog();

        }
        //Podria encapsular  la fx eliminar en una misma pero me dio pja (0
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            { 
            
                
              DialogResult respuesta= MessageBox.Show("Eliminar","ESTAS APUNTO DE ELIMINAR UN POKEMON",MessageBoxButtons.YesNo,MessageBoxIcon.Warning);
                if (DialogResult.Yes == respuesta)
                {
                    pokemon pok = (pokemon)datos.CurrentRow.DataBoundItem;
                    pokemondata pokdata = new pokemondata();
                    pokdata.bajafisica(pok._id);
                    cargar();
                }

            }catch(Exception ex) {

                MessageBox.Show(ex.Message.ToString());
            }
        
           
        }

        private void btnEliminarLogico_Click(object sender, EventArgs e)
        {
            try
            {
                pokemon pok = (pokemon)datos.CurrentRow.DataBoundItem;
                pokemondata pokdata = new pokemondata();
                pokdata.bajaLogica(pok._id);
                cargar();



            }
            catch(Exception ex)
            {


                MessageBox.Show(ex.Message.ToString()); 
            }
        }

  

       

        private void filtrotxt_TextChanged(object sender, EventArgs e)
        {

            List<pokemon> listaFiltrada;
            string filtro = filtrotxt.Text;

            if (filtro != "")
            {
                listaFiltrada = listasPokemos.FindAll(x => x._nombre.ToUpper().Contains(filtro.ToUpper()) || x.tipo._Descripcion.ToUpper().Contains(filtro.ToUpper()));

            }
            else
            {

                listaFiltrada = listasPokemos;
            }


            datos.DataSource = null;
            datos.DataSource = listaFiltrada;
            ocultarColumnas();

        }

        private void btnFiltroAvanzado_Click(object sender, EventArgs e)
        {
            
            if (contadorFiltroAvanzado % 2 == 0){//ME LA RE COMPLIQUE AL PEDO 
               lblCampo.Visible = true;
             lblCriterio.Visible = true;
             lblFiltro.Visible = true;
             txtFiltro.Visible = true;
             cboCampo.Visible = true;    
             cboCriterio.Visible = true;
             btnBuscarAvanzado.Visible = true;
              contadorFiltroAvanzado++;
            }
            else
            {
                lblCampo.Visible = false;
                lblCriterio.Visible = false;
                lblFiltro.Visible = false;
                txtFiltro.Visible = false;
                cboCampo.Visible = false;
                cboCriterio.Visible = false;
                btnBuscarAvanzado.Visible = false;

                contadorFiltroAvanzado--;


            }
            


        }

        private void cboCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string criterio = cboCampo.Text.ToString();

            switch (criterio)
            {
                case "Número":
                    cboCriterio.Items.Clear();
                    cboCriterio.Items.Add("Mayor a");
                    cboCriterio.Items.Add("Menor a");
                    cboCriterio.Items.Add("Igual a");
                    
                break;

                case "Nombre":
                case "Descripcion":
               
                     cboCriterio.Items.Clear();
                    cboCriterio.Items.Add("Contenga");
                    cboCriterio.Items.Add("Empiece con");
                    cboCriterio.Items.Add("Termine con");

                    break;


              
            }

        }

        private void btnBuscarAvanzado_Click_1(object sender, EventArgs e)
        {

            List<pokemon> listaFiltradaAvanzada= new List<pokemon>();
            AccesoDatos datosA =new AccesoDatos();
         
            string auxSwich="a";
            string criterio= cboCriterio.Text.ToString();
            string campo = cboCampo.Text.ToString();
            string filtro =txtFiltro.Text.ToString();
            string query = "select Nombre,Numero ,p.Descripcion ,UrlImagen,e.Descripcion tipo, D.descripcion debilidad,D.Id as idDebilidad,e.Id as idtipo,p.Id,p.Activo from POKEMONS p, ELEMENTOS e, ELEMENTOS D where e.Id=p.IdTipo and d.Id=p.IdDebilidad and ";

            if (campo == "Descripcion")
            {
                auxSwich = "p.descripcion";
               

            }else if(campo == "Nombre")
            {
               
                auxSwich = "Nombre";
            }



            if (filtro != "")
            {


                try
                {

                    switch (campo)
                    {

                        case "Número":

                            switch (criterio)
                            {
                                case "Igual a":

                                    query += "numero=" + filtro;
                                    break;


                                case "Menor a":

                                    query += "numero<" + filtro;

                                    break;

                                case "Mayor a":

                                    query += "numero>" + filtro;
                                    break;






                            }

                            break;

                        case "Descripción":
                        case "Nombre":

                            //NO ME ANDA ESTO SEGURO ESTA MAL LA CONSULTA;

                            switch (criterio)
                            {

                                case "Contenga":

                                    query += "" + auxSwich + " like '%" + filtro + "%' ";


                                    break;

                                case "Empiece con":

                                    query += "" + auxSwich + " like '" + filtro + "%' ";

                                    break;

                                case "Termina con":

                                    query += "" + auxSwich + " like '" + filtro + "' ";
                                    //VA "'++'" Cuando 
                                    //filtras por tipo txt , sino podes hacerlo "++

                                    break;


                                
                            }


                            break;




                    }

                    datosA.establecerQuery(query);
                    datosA.ejecutarLectura();


                    while (datosA.Lector.Read())
                    {

                        pokemon aux = new pokemon();
                        aux._descripcion = (string)datosA.Lector["Descripcion"];
                        aux.debilidad = new elemento(); //CREO INSTANCIA Y SE LO ASIGNO A DEBILIDAD
                        aux.tipo = new elemento();
                        aux._id = (int)datosA.Lector["Id"];
                        aux._numero = (int)datosA.Lector["Numero"];
                        aux.debilidad._id = (int)datosA.Lector["idDebilidad"];
                        aux.debilidad._Descripcion = (string)datosA.Lector["debilidad"];
                        aux.tipo._id = (int)datosA.Lector["idtipo"];
                        aux.tipo._Descripcion = (string)datosA.Lector["tipo"];// PODES CREAR UNA INSTANCIA DE ELEMNTO ARRIBA Y TE AHORRAS HACERLO DENTRO DE LA CLASE.
                        aux._nombre = (string)datosA.Lector["Nombre"];
                        aux._urlImagen = (string)datosA.Lector["UrlImagen"];
                        aux.activo = (bool)datosA.Lector["Activo"];


                        listaFiltradaAvanzada.Add(aux);




                    }
                    datos.DataSource = null;
                    datos.DataSource = listaFiltradaAvanzada;
                 


                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.ToString());

                }
                finally
                {
                    datosA.cerrarConexion();
                }


            }
            else
            {
                cargar();
            }
        }

        
    }




 }

