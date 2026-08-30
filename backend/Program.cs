using Microsoft.EntityFrameworkCore;
using Session19_Api.Data;
using Session19_Api.Models;

var builder = WebApplication.CreateBuilder(args);

// --- Hosting port ------------------------------------------------------
// Locally we use the address in Properties/launchSettings.json (port 5000).
// A host like Railway instead tells us which port to use through the PORT
// environment variable, so listen on that whenever it is present.
var port = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrEmpty(port))
    builder.WebHost.UseUrls($"http://0.0.0.0:{port}");
// ----------------------------------------------------------------------

builder.Services.AddControllers();

// Register EF Core and point it at a SQLite database file (shop.db).
// The host can override the location with a DB_PATH environment variable —
// useful when the database should live on a mounted disk that survives
// redeploys, since a container's own filesystem is wiped every deploy.
var dbPath = Environment.GetEnvironmentVariable("DB_PATH") ?? "shop.db";
builder.Services.AddDbContext<AppDbContext>(o =>
    o.UseSqlite($"Data Source={dbPath}"));

// --- CORS -------------------------------------------------------------
// The web page runs in the browser at a DIFFERENT address than this API
// (for example http://localhost:8000 while the API is http://localhost:5000).
// Browsers block "cross-origin" calls unless the API says they're allowed.
// This policy lets our front end call the API during development.
builder.Services.AddCors(options =>
{
    options.AddPolicy("frontend", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod());
});
// ----------------------------------------------------------------------

var app = builder.Build();

// Create the database file if it doesn't exist yet, and seed a few
// products the first time so the page shows something straight away.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();

    if (!db.Products.Any())
    {
        db.Products.AddRange(
            new Product { Name = "Pen",       Price = 3.00,   Description = "A smooth blue ballpoint pen." },
            new Product { Name = "Notebook",  Price = 8.50,   Description = "120 pages, dotted grid." },
            new Product { Name = "Keyboard",  Price = 79.99,  Description = "Compact mechanical keyboard." },
            new Product { Name = "Mouse",     Price = 19.99,  Description = "Wireless, silent click." },
            new Product { Name = "Monitor",   Price = 149.00, Description = "24-inch full-HD display." },
            new Product { Name = "Desk Lamp", Price = 24.50,  Description = "Warm LED, adjustable arm." }
        );
        db.SaveChanges();
    }
}

// Turn the CORS policy on. This must come before MapControllers.
app.UseCors("frontend");

app.MapControllers();
app.Run();
