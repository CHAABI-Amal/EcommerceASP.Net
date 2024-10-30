using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json; // Import for JSON serialization
using System.Collections.Generic;

namespace ECommerceV1.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public string NewName { get; set; } // Property for the new name input

        public List<string> Names { get; set; } = new List<string>(); // To hold the names

        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            // On initial load, get the usernames from session
            Names = GetUsernamesFromSession();
        }

        public IActionResult OnPost()
        {
            // Retrieve the current list from session
            Names = GetUsernamesFromSession();

            // Check if NewName is not empty and add it to the list
            if (!string.IsNullOrWhiteSpace(NewName))
            {
                Names.Add(NewName); // Add the new name to the list
            }

            // Serialize the updated list to JSON
            string serializedNames = JsonSerializer.Serialize(Names);

            // Store the serialized list in session
            HttpContext.Session.SetString("usernames", serializedNames);

            // Clear the NewName property after adding
            NewName = string.Empty;

            return RedirectToPage("/Produits/ListProducts"); // Redirect to the same page to refresh
        }

        public List<string> GetUsernamesFromSession()
        {
            // Retrieve the serialized list from session
            var serializedNames = HttpContext.Session.GetString("usernames");

            // Deserialize the list from JSON
            if (!string.IsNullOrEmpty(serializedNames))
            {
                return JsonSerializer.Deserialize<List<string>>(serializedNames);
            }

            return new List<string>(); // Return an empty list if session is empty
        }
    }
}
