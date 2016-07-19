using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace Cikkek.Models
{
    public class Product
    {
       
        [Display(Name = "Cikknév")]
        public string Cikknev { get; set; }

        [Display(Name = "Cikkszám")]
        public string Cikkszam { get; set; }

        [Display(Name = "Vonalkód")]
        public string Vonalkod { get; set; }

        [Display(Name = "Mennyiségi egység")]
        public string MennyisegiEgyseg { get; set; }

        

        public Product() { }

        public Product( string Cikknev, string Cikkszam, string Vonalkod, string MennyisegiEgyseg)
        {
            
            this.Cikknev = Cikknev;
            this.Cikkszam = Cikkszam;
            this.Vonalkod = Vonalkod;
            this.MennyisegiEgyseg = MennyisegiEgyseg;
        }

        
    }
}