using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ECommerceV1.Data;
using ECommerceV1.Models;

namespace ECommerceV1.Pages.Authentification
{
    public class AuthentificationModel : PageModel
    {
        private readonly ECommerceV1.Data.ECommerceV1Context _context;

        public AuthentificationModel(ECommerceV1.Data.ECommerceV1Context context)
        {
            _context = context;
        }

        [BindProperty]
        public Login Login { get; set; } = default!;

        [BindProperty]
        public string ConfirmPassword { get; set; } = default!;

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

            // Vérification si l'email existe déjà
            var existingUser = _context.Login.FirstOrDefault(u => u.Email == Login.Email);
            if (existingUser != null)
            {
                ViewData["Login.Email"] = "L'email existe déjà.";
                return Page();
            }

            // Vérification si le mot de passe et la confirmation correspondent
            if (Login.Password != ConfirmPassword)
            {
                ViewData["ConfirmPassword"] = "Le mot de passe et la confirmation ne correspondent pas.";
                return Page();
            }

            // Si tout est correct, on enregistre l'utilisateur
            _context.Login.Add(Login);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Login");
        }
    }
}
