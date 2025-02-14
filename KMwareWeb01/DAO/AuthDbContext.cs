using KMwareWeb01.DAO.Models;
using Microsoft.EntityFrameworkCore;

namespace KMwareWeb01.DAO
{
    public class AuthDbContext : DbContext
    {
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Updates> Updates { get; set; }
        public DbSet<RunTime> RunTimes { get; set; }


        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppUser>(entity =>
            {
                entity.HasKey(e => e.UserId);
                entity.Property(e => e.UserId).ValueGeneratedOnAdd();
                entity.Property(e => e.UserId);
                entity.Property(e => e.Provider).HasMaxLength(50);
                entity.Property(e => e.NameIdentifier).HasMaxLength(500);
                entity.Property(e => e.Username).HasMaxLength(250);
                entity.Property(e => e.Password).HasMaxLength(250);
                entity.Property(e => e.Email).HasMaxLength(250);
                entity.Property(e => e.Firstname).HasMaxLength(250);
                entity.Property(e => e.Lastname).HasMaxLength(250);
                entity.Property(e => e.Mobile).HasMaxLength(250);
                entity.Property(e => e.Roles).HasMaxLength(1000);

                entity.HasData(new AppUser
                {
                    Provider = "Cookies",
                    UserId = 1,
                    Email = "k@m.com",
                    Username = "k@m.com",
                    Password = "password",
                    Firstname = "K",
                    Lastname = "M",
                    NameIdentifier = "KM",
                    Mobile = "1234567890",
                    Roles = "Admin"
                });

            });

            modelBuilder.Entity<RunTime>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.V0);
                entity.Property(e => e.V1);
                entity.Property(e => e.V2);
                entity.Property(e => e.V3);
                entity.Property(e => e.V4);
                entity.Property(e => e.V5);
                entity.Property(e => e.V6);
                entity.Property(e => e.V7);
                entity.HasData(
                    new RunTime
                        { Id = 1, V0 = 0, V1 = 0, V2 = 0, V3 = 0, V4 = 0, V5 = 0, V6 = 0, V7 = 0 },
                    new RunTime
                        { Id = 2, V0 = 0, V1 = 0, V2 = 0, V3 = 0, V4 = 0, V5 = 0, V6 = 0, V7 = 0 },
                    new RunTime
                        { Id = 3, V0 = 0, V1 = 0, V2 = 0, V3 = 0, V4 = 0, V5 = 0, V6 = 0, V7 = 0 },
                    new RunTime
                        { Id = 4, V0 = 0, V1 = 0, V2 = 0, V3 = 0, V4 = 0, V5 = 0, V6 = 0, V7 = 0 },
                    new RunTime
                        { Id = 5, V0 = 0, V1 = 0, V2 = 0, V3 = 0, V4 = 0, V5 = 0, V6 = 0, V7 = 0 },
                    new RunTime
                        { Id = 6, V0 = 0, V1 = 0, V2 = 0, V3 = 0, V4 = 0, V5 = 0, V6 = 0, V7 = 0 },
                        new RunTime
                        { Id = 7, V0 = 0, V1 = 0, V2 = 0, V3 = 0, V4 = 0, V5 = 0, V6 = 0, V7 = 0 });
            });

            modelBuilder.Entity<Updates>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.IsRainDelay);
                entity.Property(e => e.RunTimesJson).HasMaxLength(1000);
                entity.Property(e => e.TimeStamp).HasMaxLength(250);

                entity.HasData(new Updates
                {
                    Id = 1,
                    IsRainDelay = 0,
                    RunTimesJson = "{\"V0\":0,\"V1\":0,\"V2\":0,\"V3\":0,\"V4\":0,\"V5\":0,\"V6\":0,\"V7\":0}",
                    TimeStamp = "2021-01-01 00:00:00"
                });
            });
        }
    }
}
