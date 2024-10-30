using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ECommerceV1.Data;
using ECommerceV1.Models;

namespace ECommerceV1.Pages.Authentification
{
    public class IndexModel : PageModel
    {
        private readonly ECommerceV1.Data.ECommerceV1Context _context;

        public IndexModel(ECommerceV1.Data.ECommerceV1Context context)
        {
            _context = context;
        }

        public IList<Login> Login { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Login = await _context.Login.ToListAsync();
        }
    }
}
