using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceV1.Models
{
    public class Produit
    {
        // Identifiant unique du produit
        public int Id { get; set; }

        // Nom du produit
        public string Nom { get; set; }

        // Description détaillée du produit
        public string Description { get; set; }

        // Prix du produit
        [Column(TypeName = "decimal(18, 3)")]
        public decimal Prix { get; set; }
        // Quantité disponible en stock
        [Display(Name = " Quantite Stock")]
        public int QuantiteStock { get; set; }
        // URL de l'image du produit
        public string ImageUrl { get; set; }


        // Catégorie à laquelle le produit appartient (relation Many-to-One)
        // Foreign key for Categorie
        public int CategorieId { get; set; }

        // Navigation property for Categorie
        public Categorie Categorie { get; set; }


        // Catégorie à laquelle le produit appartient
        // 
        public string Rating { get; set; } = string.Empty;
    }
}
