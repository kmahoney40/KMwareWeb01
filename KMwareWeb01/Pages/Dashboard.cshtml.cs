using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KMwareWeb01.Pages
{
    public class DashboardModel : PageModel
    {
        public void OnGet()
        {
        }

        public IActionResult OnPostRedirectToIndex()
        {
            return RedirectToPage("./Index");
        }
    }
}
