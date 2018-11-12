using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JC_PROJECT.Models
{
    public class Seller
    {
        public int Identifiant { get; set; }
        public int ShopId { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public string Function { get; set; }
        public DateTime Creation { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? Modification { get; set; }
        public int? UserId { get; set; }
    }
}