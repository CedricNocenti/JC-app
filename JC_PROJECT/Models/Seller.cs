using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JC_PROJECT.Models
{
    public class Seller
    {
        public int identifiant { get; set; }
        public int shopId { get; set; }
        public string nom { get; set; }
        public string prenom { get; set; }
        public string telephone { get; set; }
        public string email { get; set; }
        public string function { get; set; }
        public DateTime creation { get; set; }
        public DateTime modification { get; set; }

        public Shop Magasin { get; set; }
}
}