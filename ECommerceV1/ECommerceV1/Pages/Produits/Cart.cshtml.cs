using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using ECommerceV1.Models;

namespace ECommerceV1.Pages.Produits
{
    public class CartModel : PageModel
    {
        // Liste des articles dans le panier
        public List<CartItem> CartItems { get; set; } = new List<CartItem>();

        // Sous-total du panier
        public decimal SubTotal { get; set; }

        // Charge les articles du panier depuis la session et calcule le sous-total
        public void OnGet()
        {
            CartItems = GetCartFromSession();
            CalculateSubTotal();
        }

        // Met à jour la quantité d'un produit dans le panier
        public IActionResult OnPostUpdateQuantity(int produitId, int quantity)
        {
            var cart = GetCartFromSession(); // Récupère le panier de la session
            var productToUpdate = cart.FirstOrDefault(p => p.ProduitId == produitId);

            // Vérifie la disponibilité en stock avant d'actualiser la quantité
            if (productToUpdate != null)
            {
                if (quantity > productToUpdate.Produit.QuantiteStock)
                {
                    ModelState.AddModelError("", "Quantité maximale atteinte."); // Message d'erreur si trop de stock
                    CartItems = cart; // Récupère le panier pour la vue
                    CalculateSubTotal(); // Recalcule le sous-total
                    return Page(); // Retourne à la page sans redirection
                }
                else if (quantity > 0)
                {
                    productToUpdate.Quantity = quantity; // Met à jour la quantité si conditions valides
                    SaveCartToSession(cart); // Sauvegarde les modifications dans la session
                    CartItems = cart;
                    CalculateSubTotal(); // Recalcule le sous-total
                }
            }

            return RedirectToPage();
        }


        // Supprime un produit du panier
        public IActionResult OnPostRemoveFromCart(int produitId)
        {
            var cart = GetCartFromSession(); // Récupère le panier de la session
            var productToRemove = cart.FirstOrDefault(p => p.ProduitId == produitId);

            if (productToRemove != null)
            {
                cart.Remove(productToRemove); // Supprime l'article si trouvé dans le panier
                SaveCartToSession(cart); // Sauvegarde le panier mis à jour dans la session
                CalculateSubTotal(); // Recalcule le sous-total
            }

            return RedirectToPage();
        }

        // Calcule le sous-total en fonction de la quantité et du prix de chaque article
        private void CalculateSubTotal()
        {
            SubTotal = CartItems.Sum(item => item.Quantity * item.Produit.Prix);
        }

        // Récupère le panier de la session (ou un panier vide si session vide)
        private List<CartItem> GetCartFromSession()
        {
            var cartJson = HttpContext.Session.GetString("cart");
            return string.IsNullOrEmpty(cartJson) ? new List<CartItem>() : JsonSerializer.Deserialize<List<CartItem>>(cartJson);
        }

        // Sauvegarde le panier mis à jour dans la session
        private void SaveCartToSession(List<CartItem> cart)
        {
            var cartJson = JsonSerializer.Serialize(cart); // Sérialise le panier en JSON
            HttpContext.Session.SetString("cart", cartJson); // Sauvegarde le JSON dans la session
        }
    }
}
