using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JC_PROJECT.Models
{
    public class Customer
    {
        public int Identifiant { get; set; }
        public string Prenom { get; set; }
        public string Nom { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public string CP { get; set; }
        public string Ville { get; set; }
        public string Rue { get; set; }
        public DateTime DateCreation { get; set; }
        public DateTime? Modification { get; set; }
        public int? UserId { get; set; }
    }
}