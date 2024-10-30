using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ECommerceV1.Models;

namespace ECommerceV1.Data
{
    public class ECommerceV1Context : DbContext
    {
        public ECommerceV1Context (DbContextOptions<ECommerceV1Context> options)
            : base(options)
        {
        }

        //knzid ay model darnah  Specifies which entities are included in the  data model
        public DbSet<ECommerceV1.Models.Produit> Produit { get; set; } = default!;
        public DbSet<ECommerceV1.Models.Login> Login { get; set; } = default!;
        public DbSet<ECommerceV1.Models.Categorie> Categorie { get; set; } = default!;



        //to configure the relationships,it defines a one-to-many relationship
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Produit>()
                .HasOne(p => p.Categorie)       // A 'Produit' has one 'Categorie'
                .WithMany(c => c.Produits)      // A 'Categorie' has many 'Produits'
                .HasForeignKey(p => p.CategorieId);  // 'CategorieId' in 'Produit' is the foreign key

            base.OnModelCreating(modelBuilder);
        }

    }
}
