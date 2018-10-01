using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JC_PROJECT.Models
{
    public class Shop
    {
        public int Identifiant { get; set; }
        public string Nom { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Rue { get; set; }
        public string CodePostale { get; set; }
        public string Ville { get; set; }
        public DateTime Creation { get; set; }
        public string Createur { get; set; }
        public string StatutLegal { get; set; }
        public int NombreEmploye { get; set; }
        public int TurnOverRange1 { get; set; }
        public int TurnOverRange2 { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime Modification { get; set; }
        
        public virtual ICollection<Product> Products { get; set; }
    }
}