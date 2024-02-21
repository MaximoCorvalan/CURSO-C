using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dominio;
using Negocios;

namespace presentación
{
    public partial class formPrincipal : Form
    {
        private List<Articulo> articulosLista;
        private ArticuloData articuloNegocio = new ArticuloData();
        

        private int  cantClicksFiltro=0;
        private int cantClickDetalles = 0;                                 //TAL VEZ ME CONVIENE HACER DOS PROP CLICKS UNO PARA 
                                                                           // DETALLES Y OTRO PARA FILTRO
        public formPrincipal()
        {
            InitializeComponent();
          
        }

        private void formPrincipal_Load(object sender, EventArgs e)
        {
            cargar();
            cboCampo.Items.Add("NOMBRE");
            cboCampo.Items.Add("PRECIO");
            cboCampo.Items.Add("CATEGORIA");
            cboCampo.Items.Add("MARCA");
            cboCampo.Items.Add("CODIGO");


        }

        private void cargar()
        {
            try
            {
                articulosLista = articuloNegocio.listarArticulos();
                dgvArt.DataSource = articulosLista;
                ocultarColumnas();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
           
        }
      
        private void  cargarImagen(string img)
        { 

            try
            {
                imagenArt.Load(img);

            }
            catch (Exception ex)
            {
                imagenArt.Load("https://static.vecteezy.com/system/resources/previews/005/723/771/non_2x/photo-album-icon-image-symbol-or-no-image-flat-design-on-a-white-background-vector.jpg");
            }

        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            
            
           DialogResult respuesta = MessageBox.Show("¿Estas seguro de esto?", "ELIMINAR ARTICULO", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if(respuesta == DialogResult.Yes)
            {
                Articulo art = (Articulo)dgvArt.CurrentRow.DataBoundItem;
                articuloNegocio.EliminarArticulo(art._Id);
 
            }
            cargar();
        }

        private void dgvArt_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvArt.CurrentRow.DataBoundItem != null )
            {
                Articulo artaux = (Articulo)dgvArt.CurrentRow.DataBoundItem;
                cargarImagen(artaux._imagenUrl);
                lblCodArtRsp.Text = artaux.Codigo.ToUpper();
                lblcategoriaRsp.Text = artaux.Categoria._Descripcion.ToUpper();

            }

        }


        private void btnFiltrar_Click_1(object sender, EventArgs e) 
        {

            

            if (cantClicksFiltro == 1)
            {
               
                cambiarVisibilidad(false, 1);
                cambiarVisibilidad(true, 2);
                cargar();
                btnFiltrar.BackColor = Color.White;
                cantClicksFiltro--;
              


            }
            else 
            {
                btnFiltrar.BackColor = Color.PaleTurquoise;
                cambiarVisibilidad(true,1);
                cantClicksFiltro++;
              

            }
          



        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            AltaForm formSecundario = new AltaForm();
            formSecundario.ShowDialog();
            cargar();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            Articulo art = (Articulo)dgvArt.CurrentRow.DataBoundItem ;
            AltaForm formSecundario = new AltaForm(art);
            formSecundario.ShowDialog();
            cargar();

        }


        private void btnDetalles_Click(object sender, EventArgs e)
        {
            if (cantClickDetalles== 1)
            {
                cambiarVisibilidad(false,0);
                btnDetalles.BackColor = Color.White;
                 cantClickDetalles--;
            }
            else 
            {

                cambiarVisibilidad(true,0);
          
                btnDetalles.BackColor = Color.PaleTurquoise;

                cantClickDetalles++;
            }
           





        }

        private void cboCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                switch (cboCampo.Text)
                {

                    case "MARCA":

                        cboCriterio.DataSource = null;
                        cboCriterio.Items.Clear();     
                        MarcaData marcaData = new MarcaData();
                        cboCriterio.DataSource = marcaData.listarMarcas();

                        txtFiltro.Enabled = false;
                        lblFiltro.Enabled = false;



                        break;

                    case "NOMBRE":
                        cboCriterio.DataSource = null;
                        cboCriterio.Items.Clear();
                        cboCriterio.Items.Add("COMIENZA CON");
                        cboCriterio.Items.Add("TERMINA CON");
                        cboCriterio.Items.Add("CONTIENE");
                        txtFiltro.Enabled = true;
                        lblFiltro.Enabled = true;

                        break;


                    case "PRECIO":
                        cboCriterio.DataSource = null;
                       cboCriterio.Items.Clear();
                        cboCriterio.Items.Add("MAYOR A");
                        cboCriterio.Items.Add("MENOR A");
                        cboCriterio.Items.Add("IGUAL A");
                        txtFiltro.Enabled = true;
                        lblFiltro.Enabled = true;

                        break;


                    case "CATEGORIA":
                        cboCriterio.DataSource = null;
                      cboCriterio.Items.Clear();
                        CategoriaData categoriaData = new CategoriaData();
                        cboCriterio.DataSource = categoriaData.listarCategorias();
                        txtFiltro.Enabled = false;
                        lblFiltro.Enabled = false;


                        break;


                    case "CODIGO":

                        cboCriterio.DataSource = null;
                        cboCriterio.Items.Clear();
                        cboCriterio.Items.Add("COMIENZA CON");
                        cboCriterio.Items.Add("TERMINA CON");
                        cboCriterio.Items.Add("CONTIENE");
                        txtFiltro.Enabled = true;
                        lblFiltro.Enabled = true;

                        break;

                }
            }
            catch(Exception EX)
            {
                MessageBox.Show(EX.ToString());
            }

        }
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!validar()) return;

                string campo = cboCampo.SelectedItem.ToString();
                string criterio = cboCriterio.SelectedItem.ToString();
                string filtro = cambiarPuntoPorComa(txtFiltro.Text);
                //REALICE ESTA METODO PORQUE A LA HORA DE MANDAR LA CONSULTA NO ME ACEPTABA LA COMA.
                //PERO CUANDO AGREGABA UN ARTICULO NO ME TOMABA LA COMA PERO SI EL PUNTO , ENTONCES OPTE 
                //POR INTERCAMBIAR LA , POR EL .
             
                
                dgvArt.DataSource = articuloNegocio.BuscadaFiltrada(campo, criterio, filtro);
                  
                
                if(dgvArt.Rows.Count==0)
                {
                 
                    cambiarVisibilidad(false, 2);
                }
                else
                {
                    cambiarVisibilidad(true, 2);
                }

              


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }








        }

        //METODOS AUX
        private bool soloNumeros( string cadena)
        {


            foreach (char c in cadena)
            {
                if (c == ',')
                {
                   cadena=cadena.Replace(',', '.');
                    

                }
                if (!char.IsDigit(c) && !(c ==',') )
                {
                  
                    lblFiltro.ForeColor = Color.Red;
                    MessageBox.Show("SOLO DEBE INGRESAR NUMEROS", "ATENCION", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }

            lblFiltro.ForeColor = Color.Black;
            return true;
        }

        private bool sinCaracterEspecial(string cadena)
        {
            foreach (char s in cadena)
            {
                if (!char.IsLetterOrDigit(s))
                {

                    lblFiltro.ForeColor= Color.Red;

                    MessageBox.Show("SOLO DEBE INGRESAR NUMEROS Y LETRAS", "ATENCION", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;


                }
                else
                {
                    lblFiltro.ForeColor = Color.Black;
                }
                    
            }

            return true;

        }
        private bool validar()
           
        {
            
          if(!camposVacios()) { return false;}

             
           

            if (cboCampo.SelectedIndex == 1 && !soloNumeros( txtFiltro.Text))
            {
                return false;
            }

            if ((cboCampo.SelectedIndex == 0 || cboCampo.SelectedIndex == 4) && !(sinCaracterEspecial(txtFiltro.Text)))
            {
                return false;
            }
            


           
            

         
            return true;

        }
       
        private void cambiarVisibilidad(bool estado,int lugar)
        {

            if(lugar == 0)
            {
                lblcategoriaRsp.Visible = estado;
                lblCodArtRsp.Visible = estado;
                lblCategoria.Visible = estado;
                lblCodigo.Visible = estado;
            }
            else if(lugar == 1)
            {
                btnBuscar.Visible = estado;
                cboCampo.Visible = estado;
                cboCriterio.Visible = estado;
                txtFiltro.Visible = estado;
                lblCampo.Visible = estado;
                lblCriterio.Visible = estado;
                lblFiltro.Visible = estado;

            }else if(lugar == 2)
            {
                btnModificar.Enabled = estado;
                btnDetalles.Enabled = estado;
                btnEliminar.Enabled = estado;
            }
           
        }

        private bool camposVacios()
        {
            bool band = true;
            if (cboCampo.SelectedIndex == -1)//CAMPO
            {
                lblCampo.ForeColor = Color.Red;
                band = false;
            }
            else { lblCampo.ForeColor = Color.Black; }

            if (cboCriterio.SelectedIndex == -1)
            {
                lblCriterio.ForeColor = Color.Red;
                band = false;
            }
            else { lblCriterio.ForeColor = Color.Black; }


            if (string.IsNullOrEmpty(txtFiltro.Text) && !(cboCampo.SelectedIndex == 2 || cboCampo.SelectedIndex == 3))
            {

                lblFiltro.ForeColor = Color.Red;

                band = false;
            }
            else
            {
                lblFiltro.ForeColor = Color.Black;
            }

            if (band == false)
            {
                MessageBox.Show("COMPLETE LOS CAMPOS EN ROJO", "ATENCION", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private void ocultarColumnas()
        {
            dgvArt.Columns["_id"].Visible = false;
            dgvArt.Columns["_imagenUrl"].Visible = false;
            dgvArt.Columns["Codigo"].Visible = false;
            dgvArt.Columns["categoria"].Visible = false;


        }

        private string cambiarPuntoPorComa(string cadena)
        {

            foreach (char c in cadena)
            {
                if (c == ',')
                {
                    cadena = cadena.Replace(',', '.');


                }

            }
            return cadena;
        }   
    }
}
