
using ECommerceV1.Models;

namespace ECommerceV1.Models
{
    // Define a CartItem model to handle grouped items
    public class CartItem
    {
        public int Id { get; set; }

   
        public decimal sousPrix { get; set; }

        public int Quantity { get; set; }

        public int QuantiteStock { get; set; }

        // Relation with Produit
        public int ProduitId { get; set; } // Foreign Key to Produit
        public Produit Produit { get; set; } // Navigation Property
    }


    // 
    
}
