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
        private readonly ECommerceV1Context _context; // Contexte de la base de donn�es

        public UserModel(ECommerceV1Context context)
        {
            _context = context; // Injection du contexte dans le constructeur
        }

        public IList<Produit> Produit { get; set; } = default!; // Liste des produits disponibles

        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; } // Cha�ne de recherche pour filtrer les produits par nom

        public SelectList? Descriptions { get; set; } // Liste d�roulante des descriptions des produits

        [BindProperty(SupportsGet = true)]
        public string? ProduitDescription { get; set; } // Description du produit pour filtrer

        public SelectList? Categories { get; set; } // Liste d�roulante des cat�gories
        [BindProperty(SupportsGet = true)]
        public int? CategorieId { get; set; } // Identifiant de la cat�gorie pour filtrer

        public async Task OnGetAsync()
        {
            // Requ�te pour obtenir les noms des produits
            IQueryable<string> genreQuery = from m in _context.Produit
                                            orderby m.Nom
                                            select m.Nom;

            // Requ�te pour obtenir les cat�gories
            var categoryQuery = from c in _context.Categorie
                                orderby c.Nom
                                select c;

            var Produits = _context.Produit.AsQueryable(); // Initialisation de la requ�te pour les produits

            // Filtrer par cha�ne de recherche
            if (!string.IsNullOrEmpty(SearchString))
            {
                Produits = Produits.Where(s => s.Nom.Contains(SearchString));
            }

            // Filtrer par description
            if (!string.IsNullOrEmpty(ProduitDescription))
            {
                Produits = Produits.Where(x => x.Nom == ProduitDescription);
            }

            // Filtrer par cat�gorie
            if (CategorieId.HasValue)
            {
                Produits = Produits.Where(p => p.CategorieId == CategorieId);
            }

            // Initialiser les listes d�roulantes pour la vue
            Descriptions = new SelectList(await genreQuery.Distinct().ToListAsync());
            Categories = new SelectList(await categoryQuery.ToListAsync(), "Id", "Nom");
            Produit = await Produits.ToListAsync(); // R�cup�rer les produits apr�s filtrage
        }

        public void AddProduit(Produit p, int qt)
        {
            // R�cup�rer le panier depuis la session
            List<CartItem> cart = GetCartFromSession();
            CartItem cartItem = cart.FirstOrDefault(ci => ci.ProduitId == p.Id);

            if (cartItem != null)
            {
                cartItem.Quantity += qt; // Augmenter la quantit� si le produit existe d�j� dans le panier
            }
            else
            {
                cart.Add(new CartItem
                {
                    ProduitId = p.Id,
                    sousPrix = p.Prix * qt, // Calculer le prix total pour la quantit� ajout�e
                    Quantity = qt,
                    Produit = p
                });
            }

            SaveCartToSession(cart); // Sauvegarder le panier mis � jour dans la session
        }

        public async Task<IActionResult> OnPostAddToCartAsync(int id, int quantity)
        {
            // Recherche du produit � ajouter au panier
            var product = await _context.Produit.FindAsync(id);

            if (product == null)
            {
                return NotFound(); // Retourner une erreur si le produit n'existe pas
            }

            if (product.QuantiteStock < quantity)
            {
                ModelState.AddModelError("", "Stock insuffisant disponible."); // V�rifier le stock disponible
                return Page();
            }

            AddProduit(product, quantity); // Ajouter le produit au panier
            return RedirectToPage(); // Rediriger vers la m�me page
        }

        private List<CartItem> GetCartFromSession()
        {
            var cartJson = HttpContext.Session.GetString("cart"); // R�cup�rer le panier JSON de la session

            if (!string.IsNullOrEmpty(cartJson))
            {
                return JsonSerializer.Deserialize<List<CartItem>>(cartJson); // D�s�rialiser le panier si non vide
            }

            return new List<CartItem>(); // Retourner un panier vide si la session est vide
        }

        private void SaveCartToSession(List<CartItem> cart)
        {
            var cartJson = JsonSerializer.Serialize(cart); // S�rialiser le panier en JSON
            HttpContext.Session.SetString("cart", cartJson); // Sauvegarder le panier dans la session
        }
    }
}
