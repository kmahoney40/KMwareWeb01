using KMwareWeb01.DAO.Models;
using KMwareWeb01.DAO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace KMwareWeb01.Pages
{
    public class DashboardModel : PageModel
    {
        private readonly AuthDbContext _context;
        private readonly ILogger<IndexModel> _logger;

        [BindProperty]
        public string FirstName { get; set; }

        [BindProperty]
        public int Quantity { get; set; }

        [BindProperty]
        public List<List<int>> Quantities { get; set; }// = Enumerable.Repeat(1, 7).ToList();//new List<int>();

        [BindProperty]
        public List<RunTimes> RunTimes { get; set; }

        [BindProperty]
        public string LastUpdate { get; set; }


        public DashboardModel(AuthDbContext context, ILogger<IndexModel> logger)
        {
            _logger = logger;
            _context = context;
        }

        public void OnGet()
        {
            _logger.LogInformation("ENTERED Dashboard OnGet");

            RunTimes = _context.RunTimes.Where(r => r.Id < 8).ToList();

            var dataSource = _context.Database.GetDbConnection().DataSource;
            var dbPath = Path.Combine(Directory.GetCurrentDirectory(), dataSource);
            _logger.LogInformation($"Database Path: {dbPath}");


        }

        public IActionResult OnPostRedirectToIndex()
        {
            return RedirectToPage("./Index");
        }

        public PageResult OnPost()
        {
            var runTimes = _context.RunTimes.ToList();

            runTimes.ForEach(r => _logger.LogInformation($"RunTimes: {r.Id}, {r.V0}, {r.V1}, {r.V2}, {r.V3}, {r.V4}, {r.V5}, {r.V6}, {r.V7}"));
            RunTimes.ForEach(r => _logger.LogInformation($"RunTimes: {r.Id}, {r.V0}, {r.V1}, {r.V2}, {r.V3}, {r.V4}, {r.V5}, {r.V6}, {r.V7}"));

            var idx = 0;
            foreach (var rt in RunTimes.Where(r => r.Id < 8))
            {
                runTimes[idx].Id = idx + 1;
                runTimes[idx].V0 = rt.V0;
                runTimes[idx].V1 = rt.V1;
                runTimes[idx].V2 = rt.V2;
                runTimes[idx].V3 = rt.V3;
                runTimes[idx].V4 = rt.V4;
                runTimes[idx].V5 = rt.V5;
                runTimes[idx].V6 = rt.V6;
                runTimes[idx].V7 = rt.V7;
                ++idx;
            }

            runTimes.ForEach(r => _logger.LogInformation($"RunTimes: {r.Id}, {r.V0}, {r.V1}, {r.V2}, {r.V3}, {r.V4}, {r.V5}, {r.V6}, {r.V7}"));

            return Page();
        }
    }
}
