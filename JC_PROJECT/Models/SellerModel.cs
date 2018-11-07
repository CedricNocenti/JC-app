using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JC_PROJECT.Models
{
    public class SellerModel
    {
        public int Identifiant { get; set; }
        public int IdentifiantMagasin { get; set; }
        public string Prenom { get; set; }
        public string Nom { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public string Fonction { get; set; }
        public DateTime DateCreation { get; set; }
        public DateTime DateModification { get; set; }

        public Shop Magasin { get; set; }
    }
}