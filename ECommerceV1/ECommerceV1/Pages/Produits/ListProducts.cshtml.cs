using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ECommerceV1.Data;
using ECommerceV1.Models;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ECommerceV1.Pages.Produits
{
    public class UserModel : PageModel
    {
        private readonly ECommerceV1Context _context; // Contexte de la base de données

        public UserModel(ECommerceV1Context context)
        {
            _context = context; // Injection du contexte dans le constructeur
        }

        public IList<Produit> Produit { get; set; } = default!; // Liste des produits disponibles

        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; } // Chaîne de recherche pour filtrer les produits par nom

        public SelectList? Descriptions { get; set; } // Liste déroulante des descriptions des produits

        [BindProperty(SupportsGet = true)]
        public string? ProduitDescription { get; set; } // Description du produit pour filtrer

        public SelectList? Categories { get; set; } // Liste déroulante des catégories
        [BindProperty(SupportsGet = true)]
        public int? CategorieId { get; set; } // Identifiant de la catégorie pour filtrer

        public async Task OnGetAsync()
        {
            // Requête pour obtenir les noms des produits
            IQueryable<string> genreQuery = from m in _context.Produit
                                            orderby m.Nom
                                            select m.Nom;

            // Requête pour obtenir les catégories
            var categoryQuery = from c in _context.Categorie
                                orderby c.Nom
                                select c;

            var Produits = _context.Produit.AsQueryable(); // Initialisation de la requête pour les produits

            // Filtrer par chaîne de recherche
            if (!string.IsNullOrEmpty(SearchString))
            {
                Produits = Produits.Where(s => s.Nom.Contains(SearchString));
            }

            // Filtrer par description
            if (!string.IsNullOrEmpty(ProduitDescription))
            {
                Produits = Produits.Where(x => x.Nom == ProduitDescription);
            }

            // Filtrer par catégorie
            if (CategorieId.HasValue)
            {
                Produits = Produits.Where(p => p.CategorieId == CategorieId);
            }

            // Initialiser les listes déroulantes pour la vue
            Descriptions = new SelectList(await genreQuery.Distinct().ToListAsync());
            Categories = new SelectList(await categoryQuery.ToListAsync(), "Id", "Nom");
            Produit = await Produits.ToListAsync(); // Récupérer les produits après filtrage
        }

        public void AddProduit(Produit p, int qt)
        {
            // Récupérer le panier depuis la session
            List<CartItem> cart = GetCartFromSession();
            CartItem cartItem = cart.FirstOrDefault(ci => ci.ProduitId == p.Id);

            if (cartItem != null)
            {
                cartItem.Quantity += qt; // Augmenter la quantité si le produit existe déjà dans le panier
            }
            else
            {
                cart.Add(new CartItem
                {
                    ProduitId = p.Id,
                    sousPrix = p.Prix * qt, // Calculer le prix total pour la quantité ajoutée
                    Quantity = qt,
                    Produit = p
                });
            }

            SaveCartToSession(cart); // Sauvegarder le panier mis à jour dans la session
        }

        public async Task<IActionResult> OnPostAddToCartAsync(int id, int quantity)
        {
            // Recherche du produit à ajouter au panier
            var product = await _context.Produit.FindAsync(id);

            if (product == null)
            {
                return NotFound(); // Retourner une erreur si le produit n'existe pas
            }

            if (product.QuantiteStock < quantity)
            {
                ModelState.AddModelError("", "Stock insuffisant disponible."); // Vérifier le stock disponible
                return Page();
            }

            AddProduit(product, quantity); // Ajouter le produit au panier
            return RedirectToPage(); // Rediriger vers la même page
        }

        private List<CartItem> GetCartFromSession()
        {
            var cartJson = HttpContext.Session.GetString("cart"); // Récupérer le panier JSON de la session

            if (!string.IsNullOrEmpty(cartJson))
            {
                return JsonSerializer.Deserialize<List<CartItem>>(cartJson); // Désérialiser le panier si non vide
            }

            return new List<CartItem>(); // Retourner un panier vide si la session est vide
        }

        private void SaveCartToSession(List<CartItem> cart)
        {
            var cartJson = JsonSerializer.Serialize(cart); // Sérialiser le panier en JSON
            HttpContext.Session.SetString("cart", cartJson); // Sauvegarder le panier dans la session
        }
    }
}
