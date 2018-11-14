using JC_PROJECT.App_Start;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JC_PROJECT.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Courrier électronique")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Mémoriser ce navigateur ?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Courrier électronique")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Adresse mail")]
        [EmailAddress]
        public string Email { get; set; }

       

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Mot de passe")]
        public string Password { get; set; }

        [Display(Name = "Mémoriser le mot de passe ?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        public string Role { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Adresse email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Prénom")]
        [StringLength(50, ErrorMessage = "La chaîne {0} doit comporter au maximum {2} caractères.")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Nom")]
        [StringLength(50, ErrorMessage = "La chaîne {0} doit comporter au maximum {2} caractères.")]
        public string JLastName { get; set; }

        [RequiredIf("Role", "Client", "Attention, Code Postal requis!")]
        [Display(Name = "Code Postal")]
        [StringLength(5, ErrorMessage = "La chaîne {0} doit comporter au maximum {2} caractères.")]
        public string PostalCode { get; set; }

        [RequiredIf("Role", "Client", "Attention, adresse requise!")]
        [Display(Name = "Rue")]
        [StringLength(50, ErrorMessage = "La chaîne {0} doit comporter au maximum {2} caractères.")]
        public string Street { get; set; }

        [RequiredIf("Role", "Client", "Attention, Ville requise!")]
        [Display(Name = "Ville")]
        [StringLength(50, ErrorMessage = "La chaîne {0} doit comporter au maximum {2} caractères.")]
        public string City { get; set; }

        [Phone]
        [DataType(DataType.PhoneNumber)]
        [Display(Name ="Téléphone portable")]
        public string PhoneNumber { get; set; }

        public Int32 CreatedBy { get; set; }

        [RequiredIf("Role", "Vendeur", "Attention, fonction requise!")]
        [StringLength(50, ErrorMessage = "La chaîne {0} doit comporter au moins {2} caractères.")]
        [Display(Name="Fonction")]
        public string Function { get; set; }

        [RequiredIf("Role", "Vendeur", "Attention, nom du magasin requis!")]
        [StringLength(50, ErrorMessage = "La chaîne {0} doit comporter au moins {2} caractères.")]
        [Display(Name = "Nom du magasin")]
        public string ShopName { get; set; }

       
        [StringLength(255, ErrorMessage = "La chaîne {0} doit comporter au moins {2} caractères.")]
        [Display(Name = "Description du magasin")]
        [DataType(DataType.MultilineText)]
        public string ShopDescription { get; set; }

        [RequiredIf("Role","Vendeur","Nombre d'employés du magasin requis!")]
        [Display(Name = "Description du magasin")]

        public Int32 ShopNbEmployees { get; set; }


        [Required]
        [StringLength(100, ErrorMessage = "La chaîne {0} doit comporter au moins {2} caractères.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Mot de passe")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmer le mot de passe ")]
        [Compare("Password", ErrorMessage = "Les mots de passe ne correspondent pas")]
        public string ConfirmPassword { get; set; }


      
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Courrier électronique")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "La chaîne {0} doit comporter au moins {2} caractères.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Mot de passe")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmer le mot de passe")]
        [Compare("Password", ErrorMessage = "Les mots de passe ne correspondent pas")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "E-mail")]
        public string Email { get; set; }
    }
}
