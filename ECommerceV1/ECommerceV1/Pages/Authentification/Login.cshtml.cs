using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http; // For session management
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ECommerceV1.Data;
using ECommerceV1.Models;

namespace ECommerceV1.Pages.Authentification
{
    public class LoginModel : PageModel
    {
        private readonly ECommerceV1.Data.ECommerceV1Context _context;

        public LoginModel(ECommerceV1.Data.ECommerceV1Context context)
        {
            _context = context;
        }

        [BindProperty]
        public Login Login { get; set; } = default!;

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var existingLogin = _context.Login.FirstOrDefault(l => l.Email == Login.Email);

            // Vérifiez si l'email et le mot de passe correspondent aux informations de l'administrateur
            if (Login.Email == "admin@gmail.com" && Login.Password == "admin")
            {
                // Stocker le statut de connexion dans la session pour l'administrateur

                HttpContext.Session.SetString("IsAuthenticated", "true");
                HttpContext.Session.SetString("Admin", "true");
                HttpContext.Session.SetString("UserEmail", Login.Email);

                // Rediriger vers la page d'index
                return RedirectToPage("/Index");
            }


            if (existingLogin == null)
            {
                ViewData["LoginError"] = "Email does not exist.";
                return Page();
            }

            if (!existingLogin.Checkout(Login))
            {
                ViewData["LoginError"] = "Incorrect password.";
                return Page();
            }




            // Si login est réussi, stocker le statut de connexion dans la session
            HttpContext.Session.SetString("IsAuthenticated", "true");
            HttpContext.Session.SetString("Admin", "false");
            HttpContext.Session.SetString("UserEmail", existingLogin.Email);

            // Récupérer l'URL de retour et rediriger après connexion
            var returnUrl = HttpContext.Session.GetString("ReturnUrl") ?? "Index"; // Par défaut rediriger vers la page d'accueil
            return Redirect(returnUrl);
        }

    }
}
