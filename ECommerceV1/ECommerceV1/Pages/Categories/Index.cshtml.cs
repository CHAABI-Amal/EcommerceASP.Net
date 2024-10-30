﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ECommerceV1.Data;
using ECommerceV1.Models;

namespace ECommerceV1.Pages.Categories
{
    public class IndexModel : PageModel
    {
        private readonly ECommerceV1.Data.ECommerceV1Context _context;

        public IndexModel(ECommerceV1.Data.ECommerceV1Context context)
        {
            _context = context;
        }

        public IList<Categorie> Categorie { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Categorie = await _context.Categorie.ToListAsync();
        }
    }
}