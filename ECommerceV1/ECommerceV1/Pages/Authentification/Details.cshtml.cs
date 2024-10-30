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
    public class DetailsModel : PageModel
    {
        private readonly ECommerceV1.Data.ECommerceV1Context _context;

        public DetailsModel(ECommerceV1.Data.ECommerceV1Context context)
        {
            _context = context;
        }

        public Login Login { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var login = await _context.Login.FirstOrDefaultAsync(m => m.Id == id);
            if (login == null)
            {
                return NotFound();
            }
            else
            {
                Login = login;
            }
            return Page();
        }
    }
}
