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

        [BindProperty]
        public static int FromSync { get; set; }

        [BindProperty]
        public string Message { get; set; }

        public DashboardModel(AuthDbContext context, ILogger<IndexModel> logger)
        {
            _logger = logger;
            _context = context;
            RunTimes = _context.RunTimes.ToList();
            FromSync = 0;
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

        public PageResult OnPostUpdate()
        {
            RunTimes = _context.RunTimes.ToList();

            //runTimes.ForEach(r => _logger.LogInformation($"runTimes: {r.Id}, {r.V0}, {r.V1}, {r.V2}, {r.V3}, {r.V4}, {r.V5}, {r.V6}, {r.V7}"));
            RunTimes.ForEach(r => _logger.LogInformation($"RunTimes: {r.Id}, {r.V0}, {r.V1}, {r.V2}, {r.V3}, {r.V4}, {r.V5}, {r.V6}, {r.V7}"));

            var idx = 0;
            foreach (var rt in RunTimes.Where(r => r.Id < 8))
            {
                RunTimes[idx].Id = idx + 1;
                RunTimes[idx].V0 = rt.V0;
                RunTimes[idx].V1 = rt.V1;
                RunTimes[idx].V2 = rt.V2;
                RunTimes[idx].V3 = rt.V3;
                RunTimes[idx].V5 = rt.V5;
                RunTimes[idx].V6 = rt.V6;
                RunTimes[idx].V7 = rt.V7;
                ++idx;
            }

            RunTimes.ForEach(r => _logger.LogInformation($"RunTimes: {r.Id}, {r.V0}, {r.V1}, {r.V2}, {r.V3}, {r.V4}, {r.V5}, {r.V6}, {r.V7}"));

            return Page();
        }

        public PageResult OnPostSyncFromController()
        {
            RunTimes = _context.RunTimes.ToList();

            var lastUpdate = _context.Updates.OrderByDescending(u => u.Id).FirstOrDefault();

            //RunTimes = (lastUpdate);

            //_context.SaveChanges();


            //Check the last udate in the DB. It should be 'updated' and is JSON. Don't need to use message?? Make db acll get json and serialize.

            //Set RunTimes to lastUdate values

            FromSync++;
            var m = Message;

            var split = Message.Split("|");

            var idx = 0;
            var innerIdx = 0;
            foreach (var s in split)
            {
                _logger.LogInformation($"Message: {s}");
                if (idx > 1)
                {
                    var thisSplit = s.Split(",");
                    //foreach (var rt in s.Split(","))
                    {
                        RunTimes[innerIdx].Id = innerIdx + 1;
                        RunTimes[innerIdx].V0 = Int32.Parse(thisSplit[0]);
                        RunTimes[innerIdx].V1 = Int32.Parse(thisSplit[1]);
                        RunTimes[innerIdx].V2 = Int32.Parse(thisSplit[2]);
                        RunTimes[innerIdx].V3 = Int32.Parse(thisSplit[3]);
                        RunTimes[innerIdx].V4 = Int32.Parse(thisSplit[4]);
                        RunTimes[innerIdx].V5 = Int32.Parse(thisSplit[5]);
                        RunTimes[innerIdx].V6 = Int32.Parse(thisSplit[6]);
                        RunTimes[innerIdx].V7 = Int32.Parse(thisSplit[7]);
                        ++innerIdx;
                    }
                }
                ++idx;
            }
            _context.SaveChanges();
            //var idx = 0;
            //foreach (var rt in split)
            //{
            //    RunTimes[idx].Id = idx + 1;
            //    RunTimes[idx].V0 = rt[0];
            //    RunTimes[idx].V1 = rt[1];
            //    RunTimes[idx].V2 = rt[2];
            //    RunTimes[idx].V3 = rt[3];
            //    RunTimes[idx].V5 = rt[4];
            //    RunTimes[idx].V6 = rt[5];
            //    RunTimes[idx].V7 = rt[6];
            //    ++idx;
            //}


            return Page();
        }
    }
}
