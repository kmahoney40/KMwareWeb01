using Microsoft.EntityFrameworkCore;

namespace KMwareWeb01.DAO.Models
{
    [Index(nameof(Id), IsUnique = true)]
    public class RunTime
    {
        public int Id { get; set; }
        public int V0 { get; set; }
        public int V1 { get; set; }
        public int V2 { get; set; }
        public int V3 { get; set; }
        public int V4 { get; set; }
        public int V5 { get; set; }
        public int V6 { get; set; }
        public int V7 { get; set; }
    }
}
