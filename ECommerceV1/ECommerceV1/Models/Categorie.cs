using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ECommerceV1.Models
{
    public class Categorie
    {
        // Identifiant unique de la catégorie
        public int Id { get; set; }

        // Nom de la catégorie
        [Required]
        public string Nom { get; set; }

        // Description de la catégorie
        public string Description { get; set; }

        // URL de l'image de la catégorie
        public string ImageUrl { get; set; }

        // Liste des produits dans cette catégorie (relation One-to-Many)
        public ICollection<Produit>? Produits { get; set; } = new List<Produit>();
    }
}
