using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace dominio
{
    public class pokemon
    {
        [DisplayName("Nombre")]//ANOTACION : te permite cambiar el nombre del atributo el cual se 
        // se mostrará en pantalla
        public string _nombre { get; set; }
     
        public int _id { get; set; }
        [DisplayName("Número")]
        public int _numero {  get; set; }
        [DisplayName("Descripción")]
        public string _descripcion { get; set; }
        [DisplayName("URL imagen")]
        public string _urlImagen { get; set; }

        public bool activo { get; set; } 


        public  elemento debilidad { get; set; }

        //CREE UN ATRIBUTO PRIVADO CON SUS RESPECTIVOS GETTER Y SETTERS, ASI EVITO INSTANCIAR LA PROPIEDAD POR FUERA DE LA CLASE 
        public elemento tipo
        { get;  set; }


    }
}
