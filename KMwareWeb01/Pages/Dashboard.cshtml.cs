using KMwareWeb01.DAO.Models;
using KMwareWeb01.DAO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KMwareWeb01.Pages
{
    public class DashboardModel : PageModel
    {
        private readonly AuthDbContext _context;
        private readonly ILogger<IndexModel> _logger;
        //private readonly IHttpClientFactory _clientFactory;
        private readonly HttpClient _httpClient;

        // HttpClient lifecycle management best practices:
        // https://learn.microsoft.com/dotnet/fundamentals/networking/http/httpclient-guidelines#recommended-use
        private static HttpClient _sharedClient = new()
        {
            //KMDB need a context var here
            BaseAddress = new Uri("http://192.168.1.140:5000"),
        };

        [BindProperty]
        public string FirstName { get; set; }

        [BindProperty]
        public int Quantity { get; set; }

        [BindProperty]
        public List<List<int>> Quantities { get; set; }// = Enumerable.Repeat(1, 7).ToList();//new List<int>();

        [BindProperty]
        public List<RunTime> RunTimes { get; set; }

        [BindProperty]
        public List<string> DayOfWeek { get; set; } = new List<string> { "Mon", "Tue", "Wed", "Th", "Fri", "Sat", "Sun", "Man" };

        [BindProperty]
        public static List<string> RunTimesJson { get; set; }

        [BindProperty]
        public string LastUpdate { get; set; }

        [BindProperty]
        public static int FromSync { get; set; }

        [BindProperty]
        public string Message { get; set; }

        public DashboardModel(AuthDbContext context, IHttpClientFactory clientFactory, ILogger<IndexModel> logger)
        {
            _logger = logger;
            _context = context;
            //_clientFactory = clientFactory;
            _httpClient = clientFactory.CreateClient();
            RunTimes = _context.RunTimes.ToList();
            //RunTimesJson = new List<string>();
            FromSync = 0;
        }

        public void OnGet()
        {
            _logger.LogInformation("ENTERED Dashboard OnGet");

            RunTimes = _context.RunTimes.Where(r => r.Id < 8).ToList();

            var dataSource = _context.Database.GetDbConnection().DataSource;
            var dbPath = Path.Combine(Directory.GetCurrentDirectory(), dataSource);
            _logger.LogInformation($"Database Path: {dbPath}");

            //var httpClient = _clientFactory.CreateClient();
            //var rt = _context.RunTimes.ToList();
            using var runTimes = Task.Run(() => _httpClient.GetAsync("http://192.168.1.140:5000/runtimes")).Result;
            // Always get the values form the controller
            string content = Task.Run(() => runTimes.Content.ReadAsStringAsync()).Result.Trim();

            if (string.IsNullOrEmpty(content))
            {
                _logger.LogInformation("No content returned from the rPI");
                return;
            }
            RunTimesJson = new List<string>();
            RunTimesJson.Add(content);

            // Deserialize JSON to a 2D array
            int[][] array = JsonSerializer.Deserialize<int[][]>(content);

            RunTimes.Clear();

            // Map to a list of RunTime objects
            //List<RunTime> runTimes = new List<RunTime>();
            for (int i = 0; i < array.Length; i++)
            {
                var runTime = new RunTime
                {
                    Id = i+1,
                    V0 = array[i][0],
                    V1 = array[i][1],
                    V2 = array[i][2],
                    V3 = array[i][3],
                    V4 = array[i][4],
                    V5 = array[i][5],
                    V6 = array[i][6],
                    V7 = array[i][7]
                };
                RunTimes.Add(runTime);
            }

        }

        public IActionResult OnPostRedirectToIndex()
        {
            return RedirectToPage("./Index");
        }

        public void OnPostGarageDoor()
        {
            using var runTimes = Task.Run(() => _httpClient.GetAsync("http://192.168.1.140:5000/garage_door")).Result;
            _logger.LogInformation("ENTERED Dashboard.OnGetGarageDoor");
            return;
        }

        public void OnPostDelayWater()
        {
            using var res = Task.Run(() => _httpClient.PostAsync("http://192.168.1.140:5000/24_hour_delay", new StringContent(""))).Result;

        }

        public PageResult OnPostUpdate()
        {
            //RunTimes = _context.RunTimes.ToList();

            ////KMDB RunTimes is from the DB, not the UI. The UI is just a display of the DB values. The POST is working out and back to the rPI. 
            //// Need to get values from the UI and update the DB.
            //// Think about this some more. If the rPI is the keeper of RunTimes, then we don't need the table in the DB. We make a git to the
            //// rPI for RunTimes in the CTOR - modify on this page and send back to the rPI. The RunTimes values would remain model variables. 
            //RunTimes.ForEach(r => _logger.LogInformation($"RunTimes: {r.Id}, {r.V0}, {r.V1}, {r.V2}, {r.V3}, {r.V4}, {r.V5}, {r.V6}, {r.V7}"));

            //var idx = 0;
            string rtj = $"[[{RunTimes[0].V1},{RunTimes[0].V2},{RunTimes[0].V3},{RunTimes[0].V4},{RunTimes[0].V5},{RunTimes[0].V6},{RunTimes[0].V7}]," +
                          $"[{RunTimes[1].V1},{RunTimes[1].V2},{RunTimes[1].V3},{RunTimes[1].V4},{RunTimes[1].V5},{RunTimes[1].V6},{RunTimes[1].V7}]," +
                          $"[{RunTimes[2].V1},{RunTimes[2].V2},{RunTimes[2].V3},{RunTimes[2].V4},{RunTimes[2].V5},{RunTimes[2].V6},{RunTimes[2].V7}]," +
                          $"[{RunTimes[3].V1},{RunTimes[3].V2},{RunTimes[3].V3},{RunTimes[3].V4},{RunTimes[3].V5},{RunTimes[3].V6},{RunTimes[3].V7}]," +
                          $"[{RunTimes[4].V1},{RunTimes[4].V2},{RunTimes[4].V3},{RunTimes[4].V4},{RunTimes[4].V5},{RunTimes[4].V6},{RunTimes[4].V7}]," +
                          $"[{RunTimes[5].V1},{RunTimes[5].V2},{RunTimes[5].V3},{RunTimes[5].V4},{RunTimes[5].V5},{RunTimes[5].V6},{RunTimes[5].V7}]," +
                          $"[{RunTimes[6].V1},{RunTimes[6].V2},{RunTimes[6].V3},{RunTimes[6].V4},{RunTimes[6].V5},{RunTimes[6].V6},{RunTimes[6].V7}]" +
                          "]";

            // Convert RunTimes to a 2D array of integers (only V0 to V7)
            var array = RunTimes
                .Select(rt => new[] { rt.V0, rt.V1, rt.V2, rt.V3, rt.V4, rt.V5, rt.V6, rt.V7 })
                .ToArray();

            // Serialize to JSON
            string json = JsonSerializer.Serialize(array);

            //foreach (var rt in RunTimes.Where(r => r.Id < 8))
            //{
            //    //RunTimes[idx].Id = idx + 1;
            //    rtj = rtj + rt.V0 + "," + rt.V1 + "," + rt.V2 + "," + rt.V3 + "," + rt.V4 + "," + rt.V5 + "," + rt.V6 + "," + rt.V7 + ",";    
            //    RunTimes[idx].V0 = rt.V0;
            //    RunTimes[idx].V1 = rt.V1;
            //    RunTimes[idx].V2 = rt.V2;
            //    RunTimes[idx].V3 = rt.V3;
            //    RunTimes[idx].V5 = rt.V5;
            //    RunTimes[idx].V6 = rt.V6;
            //    RunTimes[idx].V7 = rt.V7;
            //    ++idx;
            //}

            //RunTimes.ForEach(r => _logger.LogInformation($"RunTimes: {r.Id}, {r.V0}, {r.V1}, {r.V2}, {r.V3}, {r.V4}, {r.V5}, {r.V6}, {r.V7}"));

            //KMDB convert RunTimes to RunTimesJson here
            //Serialize? must be a better way to  this.

            //Create https handler and send to controller
            //var httpClient = _clientFactory.CreateClient();
            //var content = new StringContent(JsonSerializer.Serialize(RunTimes), Encoding.UTF8, "application/json");
            //content = new StringContent(RunTimesJson[0], Encoding.UTF8, "application/json");
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            //using var response = Task.Run(() => httpClient.PostAsync("http://192.168.1.140:5000/update_runtimes", content)).Result;
            var response = Task.Run(() => _httpClient.PostAsync("http://192.168.1.140:5000/update_runtimes", content)).Result;

            return Page();
        }

        public PageResult OnPostSyncFromController()
        {
            RunTimes = _context.RunTimes.ToList();

            var lastUpdate = _context.Updates.OrderByDescending(u => u.Id).FirstOrDefault();

            //RunTimes = (lastUpdate);

            //_context.SaveChanges();


            //Check the last udate in the DB. It should be 'updated' and is JSON. Don't need to use message?? Make db call get json and serialize.

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

            var response =  GetAsync(_sharedClient);


            return Page();
        }

        static async Task GetAsync(HttpClient httpClient)
        {
            using HttpResponseMessage response = await httpClient.GetAsync("/");
            //return null;
            response.EnsureSuccessStatusCode();//.WriteRequestToConsole();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"{jsonResponse}\n");
        }
    }
}
