using dominio;
using negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using static System.Net.Mime.MediaTypeNames;

namespace practica1bdLECTURA
{
    public partial class altaPokemon : Form

    {
        private pokemon pokemon = null;
        private OpenFileDialog archivo = null;
        public altaPokemon()
        {
            InitializeComponent();
        }
        public altaPokemon(pokemon pokemon)
        {
            InitializeComponent();
            this.pokemon = pokemon;
            Text="Modifica Pokemon";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            pokemondata pokemondata = new pokemondata();
           
            //pokemon.debilidad = new elemento();
            //pokemon.tipo = new elemento(); 
            try
            {
                if (pokemon == null)
                {
                    pokemon = new pokemon();

                }
                
                    
                    pokemon._numero = int.Parse(num.Text);
                    pokemon._descripcion = descripcionTxt.Text;
                    pokemon._urlImagen = urlTxt.Text;

                    pokemon._nombre = nombretxt.Text;
                    pokemon.debilidad = (elemento)tipocombo.SelectedItem;
                    pokemon.tipo = (elemento)debilidadCombo.SelectedItem;


                if (pokemon._id != 0)
                {
                    pokemondata.modificarPokemon(pokemon);
                    MessageBox.Show("MODIFICADO  EXITOSAMENTE");

                }
                else
                {
                    pokemondata.AgregarPokemon(pokemon);
                    MessageBox.Show("AGREGADO EXITOSAMENTE");

                }

                if (archivo != null && !urlTxt.Text.ToUpper().Contains("HTTP")) ;
                {

                    File.Copy(archivo.FileName, ConfigurationManager.AppSettings["images-folder"]+archivo.SafeFileName);
                }






                Close();

            }catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            


           

        }
        private void cargarImagen( string img)
        {
            try
            {
                imgPokemon.Load(pokemon._urlImagen);
            }
            catch (Exception ex)
            {

                imgPokemon.Load("https://editorial.unc.edu.ar/wp-content/uploads/sites/33/2022/09/placeholder.png");

            }

        }

        private void altaPokemon_Load(object sender, EventArgs e)
        {
            ElementoNegocio elementoNegocio = new ElementoNegocio();
            try
            { //NO SE PARA QUE CUERNO HICE ESTO 
                tipocombo.DataSource = elementoNegocio.listarElemento();
                tipocombo.ValueMember = "_id";
                tipocombo.DisplayMember = "_descripcion";//SIRVE PARA EL CARGAR EL TIPO
                //y debilidad del pokemoon a la hora de modificar, utilizamos la clave-valor

                debilidadCombo.DataSource = elementoNegocio.listarElemento();
                debilidadCombo.ValueMember = "_id";
                debilidadCombo.DisplayMember = "_descripcion";
                if (pokemon != null)
                {
                    urlTxt.Text = pokemon._urlImagen;
                    nombretxt.Text = pokemon._nombre;
                    num.Text = pokemon._numero.ToString();
                    descripcionTxt.Text = pokemon._descripcion;
                    cargarImagen(pokemon._urlImagen);
                    tipocombo.SelectedValue = pokemon.tipo._id; //NO LO ENTIENDO BIEN , TENGO QUE REPASAR
                    debilidadCombo.SelectedValue = pokemon.debilidad._id;

                    
                }


            }catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
               
            }

        }

        private void urlTxt_Leave(object sender, EventArgs e)
        {

            try
            {
                imgPokemon.Load(urlTxt.Text);
            }
            catch(Exception ex)
            {
                imgPokemon.Load("https://editorial.unc.edu.ar/wp-content/uploads/sites/33/2022/09/placeholder.png");
            }
        }

        private void btnImgLocal_Click(object sender, EventArgs e)
        {
            archivo= new OpenFileDialog(); // ABRE LA VENTANA DE ARCHIVOS 
            archivo.Filter = "jpg|*.jpg;|png|*.png";//FILTRADO

            if (archivo.ShowDialog() == DialogResult.OK)
            {

                urlTxt.Text= archivo.FileName;


             
            }

        }
    }
}
