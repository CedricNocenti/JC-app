﻿using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using JC_PROJECT.Models;
using System.Security.Claims;
using Oracle.ManagedDataAccess.Client;
using System.Net.Http.Headers;
using System.Net.Http;

namespace JC_PROJECT.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        public const string connectionDB = "User Id=APP_DEV;Password=appDev2018!;Data Source=54.38.230.175/orcldev";
        public const string connectionDBAUTH = "User Id=AUTH_DEV;Password=authDev2018!;Data Source=54.38.230.175/orcldev";
        public const string Baseurl = "http://localhost:62000/";
        public ManageController()
        {
        }

        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Manage/Index
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Votre mot de passe a été changé."
                : message == ManageMessageId.SetPasswordSuccess ? "Votre mot de passe a été défini."
                : message == ManageMessageId.SetTwoFactorSuccess ? "Votre fournisseur d'authentification à 2 facteurs a été défini."
                : message == ManageMessageId.Error ? "Une erreur s'est produite."
                : message == ManageMessageId.AddPhoneSuccess ? "Votre numéro de téléphone a été ajouté."
                : message == ManageMessageId.RemovePhoneSuccess ? "Votre numéro de téléphone a été supprimé."
                : "";

            var userId = User.Identity.GetUserId<int>();
            var model = new IndexViewModel
            {
                HasPassword = HasPassword(),
                PhoneNumber = await UserManager.GetPhoneNumberAsync(User.Identity.GetUserId<int>()),
                TwoFactor = await UserManager.GetTwoFactorEnabledAsync(User.Identity.GetUserId<int>()),
                Logins = await UserManager.GetLoginsAsync(User.Identity.GetUserId<int>()),
                Email = await UserManager.GetEmailAsync(User.Identity.GetUserId<int>()),
                BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(User.Identity.GetUserId())
            };
            return View(model);
        }

        //
        // POST: /Manage/RemoveLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            ManageMessageId? message;
            var result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId<int>(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId<int>());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("ManageLogins", new { Message = message });
        }

        //
        // GET: /Manage/AddPhoneNumber
        public ActionResult AddPhoneNumber()
        {
            return View();
        }

        //
        // POST: /Manage/AddPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Générer le jeton et l'envoyer
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId<int>(), model.Number);
            if (UserManager.SmsService != null)
            {
                var message = new IdentityMessage
                {
                    Destination = model.Number,
                    Body = "Votre code de sécurité est : " + code
                };
                await UserManager.SmsService.SendAsync(message);
            }
            return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });
        }

        //
        // POST: /Manage/EnableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId<int>(), true);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId<int>());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // POST: /Manage/DisableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DisableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId<int>(), false);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId<int>());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // GET: /Manage/VerifyPhoneNumber
        public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId<int>(), phoneNumber);
            // Envoyer un SMS via le fournisseur SMS afin de vérifier le numéro de téléphone
            return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
        }

        //
        // POST: /Manage/VerifyPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePhoneNumberAsync(User.Identity.GetUserId<int>(), model.PhoneNumber, model.Code);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId<int>());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.AddPhoneSuccess });
            }
            //Si nous sommes arrivés là, quelque chose a échoué, réafficher le formulaire
            ModelState.AddModelError("", "La vérification du téléphone a échoué");
            return View(model);
        }

        //
        // POST: /Manage/RemovePhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemovePhoneNumber()
        {
            var result = await UserManager.SetPhoneNumberAsync(User.Identity.GetUserId<int>(), null);
            if (!result.Succeeded)
            {
                return RedirectToAction("Index", new { Message = ManageMessageId.Error });
            }
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId<int>());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", new { Message = ManageMessageId.RemovePhoneSuccess });
        }

        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId<int>(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId<int>());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }

        //
        // GET: /Manage/SetPassword
        public ActionResult SetPassword()
        {
            return View();
        }

        //
        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId<int>(), model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId<int>());
                    if (user != null)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }
                    return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                }
                AddErrors(result);
            }

            //Si nous sommes arrivés là, quelque chose a échoué, réafficher le formulaire
            return View(model);
        }

        //
        // GET: /Manage/ManageLogins
        public async Task<ActionResult> ManageLogins(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.RemoveLoginSuccess ? "La connexion externe a été supprimée."
                : message == ManageMessageId.Error ? "Une erreur s'est produite."
                : "";
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId<int>());
            if (user == null)
            {
                return View("Error");
            }
            var userLogins = await UserManager.GetLoginsAsync(User.Identity.GetUserId<int>());
            var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
            ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;
            return View(new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            });
        }

        //
        // POST: /Manage/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Demander une redirection vers le fournisseur de connexion externe afin de lier une connexion pour l'utilisateur actuel
            return new AccountController.ChallengeResult(provider, Url.Action("LinkLoginCallback", "Manage"), User.Identity.GetUserId());
        }

        //
        // GET: /Manage/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId<int>(),
                loginInfo.Login);
            return result.Succeeded ? RedirectToAction("ManageLogins") : RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
        }

        public async Task<ActionResult> ChangeEmail(SetEmailViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId<int>());
                if (user != null)
                {
                    if (user.Email != model.EmailActuel)
                    {
                        ModelState.AddModelError("EmailActuel", "L'email actuel n'est pas correct");
                    }
                    else
                    {
                        //Mettre à jour l'email et le username dans les tables AspNetIdentity (Email = UserName)
                        user.Email = model.NouvelEmail;
                        user.UserName = model.NouvelEmail;
                        var result = await UserManager.UpdateAsync(user);

                        if (result.Succeeded)
                        {
                            //Récupérer le role de l'utilisateur connecté (Client ou Vendeur) pour pouvoir mettre à jour la table JC_CUSTOMER ou JC_SELLER
                            var roles = await UserManager.GetRolesAsync(user.Id);
                            if (roles != null)
                            {
                                if (roles[0] == "Client")
                                {
                                    
                                    using (var client = new HttpClient())
                                    {
                                        //Passing service base url  
                                        client.BaseAddress = new Uri(Baseurl);
                                        var response = client.PutAsJsonAsync("api/customer/UpdateEmail/" + user.Id, model.NouvelEmail ).Result;
                                        if (response.IsSuccessStatusCode)
                                        {
                                            return RedirectToAction("Index");
                                        }
                                    }
                                }

                            }
                            else
                            {
                                if (roles[0] == "Vendeur")
                                {
                                    
                                    //Mettre jour l'email du vendeur sur la table JC_SELLER via l'api
                                    using (var client = new HttpClient())
                                    {
                                        //Passing service base url  
                                        client.BaseAddress = new Uri(Baseurl);
                                        var response = client.PutAsJsonAsync("api/seller/UpdateEmail/" + user.Id, model.NouvelEmail).Result;
                                        if (response.IsSuccessStatusCode)
                                        {
                                            return RedirectToAction("Index");
                                        }

                                    }
                                }
                            }
                        }
                        else
                        {
                            return RedirectToAction("Index", new { Message = ManageMessageId.Error });
                        }

                    }

                }
            }
            return View(model);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        #region Programmes d'assistance
        // Utilisé pour la protection XSRF lors de l'ajout de connexions externes
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId<int>());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private bool HasPhoneNumber()
        {
            var user = UserManager.FindById(User.Identity.GetUserId<int>());
            if (user != null)
            {
                return user.PhoneNumber != null;
            }
            return false;
        }

        private bool HasEmail()
        {
            var user = UserManager.FindById(User.Identity.GetUserId<int>());
            if (user != null)
            {
                return user.Email != null;
            }
            return false;
        }


        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

        #endregion
    }
}