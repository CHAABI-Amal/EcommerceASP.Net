using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ECommerceV1.Data;
using ECommerceV1.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

using System.Text.Json;

namespace ECommerceV1.Pages.Produits
{
    public class IndexModel : PageModel
    {
        private readonly ECommerceV1.Data.ECommerceV1Context _context;

        public IndexModel(ECommerceV1.Data.ECommerceV1Context context)
        {
            _context = context;
        }

        public IList<Produit> Produit { get;set; } = default!;


        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }


        [BindProperty(SupportsGet = true)]
        public int? CategorieId { get; set; }

        // Liste des catégories pour le dropdown
        public List<SelectListItem> CategorieList { get; set; }

        public IList<Produit> Produits { get; set; } // Liste des produits à afficher

        public SelectList? Descriptions { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? ProduitDescription { get; set; }


        public async Task OnGetAsync()
        {
            // Remplir la liste des catégories
            CategorieList = _context.Categorie
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Nom
                }).ToList();

            // Use LINQ to get list of genres.
            IQueryable<string> genreQuery = from m in _context.Produit
                                            orderby m.Description
                                            select m.Description;

            var ProduitsQuery = from m in _context.Produit
                                select m;

            if (!string.IsNullOrEmpty(SearchString))
            {
                ProduitsQuery = ProduitsQuery.Where(s => s.Description.Contains(SearchString) || s.Nom.Contains(SearchString));
            }

            if (!string.IsNullOrEmpty(ProduitDescription))
            {
                ProduitsQuery = ProduitsQuery.Where(x => x.Description == ProduitDescription);
            }

            if (CategorieId.HasValue)
            {
                ProduitsQuery = ProduitsQuery.Where(p => p.CategorieId == CategorieId);
            }

            Descriptions = new SelectList(await genreQuery.Distinct().ToListAsync());
            Produit = await ProduitsQuery.ToListAsync();
        }


        public async Task<IActionResult> OnPostAddToCartAsync(int id)
        {
            var product = await _context.Produit.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            // Get cart from session or create a new one
            var cart = GetCartFromSession();

            // Add the selected product to the cart
            cart.Add(product);

            // Save the updated cart to the session
            SaveCartToSession(cart);

            return RedirectToPage();
        }

        private List<Produit> GetCartFromSession()
        {
            // Retrieve the cart from session
            var cartJson = HttpContext.Session.GetString("cart");

            if (!string.IsNullOrEmpty(cartJson))
            {
                return JsonSerializer.Deserialize<List<Produit>>(cartJson);
            }

            return new List<Produit>();
        }

        private void SaveCartToSession(List<Produit> cart)
        {
            var cartJson = JsonSerializer.Serialize(cart);
            HttpContext.Session.SetString("cart", cartJson);
        }

      

    }
}
