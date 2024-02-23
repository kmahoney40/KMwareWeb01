using KMwareWeb01.DAO;
using KMwareWeb01.DAO.Models;
using KMwareWeb01.Hubs;
using KMwareWeb01.Pages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace KMwareWeb01.Controllers
{
    public class UpdateController : ControllerBase
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly AuthDbContext _context;
        private readonly IHubContext<UpdateHub> _hubContext;
        private static Update LastUpdate = new Update
        {
            IsRainDelay = 0,
            RunTimes = new List<List<int>>()
            {
                new List<int>() { 0, 2, 3, 4, 5, 6, 7, 8 },
                new List<int>() { 2, 2, 3, 4, 5, 6, 7, 8 },
                new List<int>() { 3, 2, 3, 4, 5, 6, 7, 8 },
                new List<int>() { 4, 2, 3, 4, 5, 6, 7, 8 },
                new List<int>() { 5, 2, 3, 4, 5, 6, 7, 8 },
                new List<int>() { 6, 2, 3, 4, 5, 6, 7, 8 },
                new List<int>() { 7, 2, 3, 4, 5, 6, 7, 8 },
                new List<int>() { 8, 2, 3, 4, 5, 6, 7, 8 }
            }
        };

        public UpdateController(AuthDbContext authDbContext, IHubContext<UpdateHub> hubContext, ILogger<IndexModel> logger)
        {
            _logger = logger;
            _context = authDbContext;
            _hubContext = hubContext;
        }

        [HttpGet]
        [Route("Update")]
        public async Task<IActionResult> SendMessage(string message)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", message);
            return Ok();
        }

        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody] Update update)
        {
            var now = DateTime.Now.ToString();

            var updates = _context.Updates;

            var eq = UpdatesAreEqual(update);
            if (!eq)
            {
                //var updates = _context.Updates.ToList();
                LastUpdate.IsRainDelay = update.IsRainDelay;
                DeepCopy(update.RunTimes);

                _logger.LogInformation($"UpdateController.Update RunTimes:[ {JsonSerializer.Serialize(update.RunTimes)}]");

              updates.Add(new DAO.Models.Updates
                {
                    IsRainDelay = update.IsRainDelay,
                    RunTimesJson = JsonSerializer.Serialize(update.RunTimes),
                    TimeStamp = DateTime.Now.ToString()
                });

                //updates.Add(new DAO.Models.Updates { IsRainDelay = update.IsRainDelay, RunTimesJson = JsonSerializer.Serialize(update.RunTimes) });
                _context.SaveChanges();
            }

            //var lastUpdate = new Update()
            //{
            //    IsRainDelay = update.IsRainDelay,
            //    RunTimes = update.RunTimes
            //};

            if (false)
            {
                //Need update table get the last row of Updates and if runtimes or raindelay is different then add a new row
                //if they are the same then update the last row - set the lastupdate to now
            }

            List<int> list = new List<int>() { 1, 2, 3 };

            var l1 = update.RunTimes[0];
            var l2 = update.RunTimes[1];
            var l3 = update.RunTimes[2];
            var l4 = update.RunTimes[3];
            var l5 = update.RunTimes[4];
            var l6 = update.RunTimes[5];
            var l7 = update.RunTimes[6];
            //var l8 = update.RunTimes[7];
            var s1 = string.Join(",", l1);
            var s2 = string.Join(",", l2);
            var s3 = string.Join(",", l3);
            var s4 = string.Join(",", l4);
            var s5 = string.Join(",", l5);
            var s6 = string.Join(",", l6);
            var s7 = string.Join(",", l7);
            //var s8 = string.Join(",", l8);

            var runTimesJson = JsonSerializer.Serialize(update.RunTimes);

            await _hubContext.Clients.All.SendAsync("ReceiveMessage", $"{now}|{update.IsRainDelay}|{s1}|{s2}|{s3}|{s4}|{s5}|{s6}|{s7}");

            _logger.LogInformation($"UpdateController.Update: [{update.IsRainDelay}], [{update.RunTimes}]");


            return Ok();
        }

        private bool UpdatesAreEqual(Update update)
        {
            var updateAreEqual = false;
            var listsAreEqual = LastUpdate.RunTimes.Count == update.RunTimes.Count && LastUpdate.RunTimes.Zip(update.RunTimes, (l1, l2) => l1.SequenceEqual(l2)).All(equal => equal);
            if (LastUpdate.IsRainDelay == update.IsRainDelay && listsAreEqual)
            {
                updateAreEqual = true;
            }
            else
            {
                updateAreEqual = false;
            }

            return updateAreEqual;
        }

        private void DeepCopy(List<List<int>> source)
        {
            LastUpdate.RunTimes.Clear();
            foreach (var list in source)
            {
                LastUpdate.RunTimes.Add(list);
            }
        }

    }

    //KMDB This should be in Model folder
    public class Update
    {
        //public string LastUpdate { get; set; }
        public byte IsRainDelay { get; set; }
        public List<List<int>> RunTimes { get; set; }
        //public string RunTimesJson { get; set; }
    }

}
