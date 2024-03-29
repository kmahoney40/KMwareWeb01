﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KMwareWeb01.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            _logger.LogInformation("ENTERED Index OnGet");
        }

        public IActionResult OnPostRedirectToDashboard()
        {
            return RedirectToPage("./Dashboard");
        }
    }
}
