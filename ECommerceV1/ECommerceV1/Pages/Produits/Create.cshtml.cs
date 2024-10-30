using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ECommerceV1.Data;
using ECommerceV1.Models;
using Microsoft.AspNetCore.Hosting;

namespace ECommerceV1.Pages.Produits
{
    public class CreateModel : PageModel
    {
        private readonly ECommerceV1Context _context;
        private readonly IWebHostEnvironment _host;

        public CreateModel(ECommerceV1Context context, IWebHostEnvironment host)
        {
            _context = context;
            _host = host;
        }
        // Property for categories
        public List<SelectListItem> CategorieList { get; set; }
        public IActionResult OnGet()
        {
            // Populate the category dropdown list
            CategorieList = _context.Categorie
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Nom
                }).ToList();

            return Page();
        }

        [BindProperty]
        public Produit Produit { get; set; } = default!;

        // For category dropdown list
        [BindProperty]
        public int CategorieId { get; set; }

        // For more information, see https://aka.ms/RazorPagesCRUD.

        public static string CopyFile(IFormFile file, string uploadFolder)
        {
            var extention = Path.GetExtension(file.FileName);
            string newName = Guid.NewGuid().ToString() + extention;
            var fileDest = Path.Combine(uploadFolder, newName);
            using (var fileStream = new FileStream(fileDest, FileMode.Create))
            {
                file.CopyToAsync(fileStream);
            }
            return newName;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            string webroot = _host.WebRootPath;
            var files = HttpContext.Request.Form.Files;
            string ImageFolder = @"Images\Produits";
            string UploadFolder = Path.Combine(webroot, ImageFolder);

            // Handle the image file upload
            string fileNewName = CopyFile(files[0], UploadFolder);
            Produit.ImageUrl = Path.Combine(ImageFolder, fileNewName);

            // Associate the selected category with the product
            Produit.CategorieId = CategorieId;

            _context.Produit.Add(Produit);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
