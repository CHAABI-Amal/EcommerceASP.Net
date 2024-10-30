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

        // Met � jour la quantit� d'un produit dans le panier
        public IActionResult OnPostUpdateQuantity(int produitId, int quantity)
        {
            var cart = GetCartFromSession(); // R�cup�re le panier de la session
            var productToUpdate = cart.FirstOrDefault(p => p.ProduitId == produitId);

            // V�rifie la disponibilit� en stock avant d'actualiser la quantit�
            if (productToUpdate != null)
            {
                if (quantity > productToUpdate.Produit.QuantiteStock)
                {
                    ModelState.AddModelError("", "Quantit� maximale atteinte."); // Message d'erreur si trop de stock
                    CartItems = cart; // R�cup�re le panier pour la vue
                    CalculateSubTotal(); // Recalcule le sous-total
                    return Page(); // Retourne � la page sans redirection
                }
                else if (quantity > 0)
                {
                    productToUpdate.Quantity = quantity; // Met � jour la quantit� si conditions valides
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
            var cart = GetCartFromSession(); // R�cup�re le panier de la session
            var productToRemove = cart.FirstOrDefault(p => p.ProduitId == produitId);

            if (productToRemove != null)
            {
                cart.Remove(productToRemove); // Supprime l'article si trouv� dans le panier
                SaveCartToSession(cart); // Sauvegarde le panier mis � jour dans la session
                CalculateSubTotal(); // Recalcule le sous-total
            }

            return RedirectToPage();
        }

        // Calcule le sous-total en fonction de la quantit� et du prix de chaque article
        private void CalculateSubTotal()
        {
            SubTotal = CartItems.Sum(item => item.Quantity * item.Produit.Prix);
        }

        // R�cup�re le panier de la session (ou un panier vide si session vide)
        private List<CartItem> GetCartFromSession()
        {
            var cartJson = HttpContext.Session.GetString("cart");
            return string.IsNullOrEmpty(cartJson) ? new List<CartItem>() : JsonSerializer.Deserialize<List<CartItem>>(cartJson);
        }

        // Sauvegarde le panier mis � jour dans la session
        private void SaveCartToSession(List<CartItem> cart)
        {
            var cartJson = JsonSerializer.Serialize(cart); // S�rialise le panier en JSON
            HttpContext.Session.SetString("cart", cartJson); // Sauvegarde le JSON dans la session
        }
    }
}
