using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ECommerceV1.Pages.Produits
{
    public class PaiementModel : PageModel
    {
        public void OnGet()
        {
            // Afficher la page de paiement
        }

        public IActionResult OnPost()
        {
            // Ajouter ici la logique pour traiter le paiement

            return RedirectToPage("/Produits/Confirmation"); // Rediriger vers une page de confirmation
        }
    }
}
