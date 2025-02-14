using KMwareWeb01.DAO;
using KMwareWeb01.Hubs;
using KMwareWeb01.Pages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
//var minLogLevel = int.Parse(configuration["MinLogLevel"] ?? "3");

LogEventLevel minLogLevel = (LogEventLevel)int.Parse(configuration["MinLogLevel"] ?? "3");

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .MinimumLevel.Is(minLogLevel)
    .WriteTo.Console()
    .WriteTo.Debug()
    .WriteTo.File(@"C:\log\log.txt", rollingInterval: RollingInterval.Day)
    .Enrich.FromLogContext()
    .CreateBootstrapLogger();

builder.Host.UseSerilog();

Log.Information("Program Started");

builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseSqlite(configuration.GetConnectionString("DefaultConnection"))
    .LogTo(Console.WriteLine));

builder.Services.AddHttpClient();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapHub<UpdateHub>("/Hubs/UpdateHub");



app.Run();
