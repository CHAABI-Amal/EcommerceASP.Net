using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ECommerceV1.Pages.Authentification
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            // Effacer les informations de session
            HttpContext.Session.Remove("IsAuthenticated");
            HttpContext.Session.Remove("Admin");
            HttpContext.Session.Remove("UserEmail");

            // Rediriger vers la page d'accueil ou de connexion après la déconnexion
            return RedirectToPage("/Index"); // ou RedirectToPage("/Authentification/Login") selon votre besoin
        }
    }
}
