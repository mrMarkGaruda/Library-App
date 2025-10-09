using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Library_App.Data;
using System.Text.Json.Serialization; // for ReferenceHandler
using Library_App.Services;
var builder = WebApplication.CreateBuilder(args);

// Prefer connection string from env or user secrets. If missing, fall back to appsettings.json.
var connectionString = builder.Configuration.GetConnectionString("Library_AppContextConnection")
                      ?? Environment.GetEnvironmentVariable("ConnectionStrings__Library_AppContextConnection")
                      ?? throw new InvalidOperationException("Connection string 'Library_AppContextConnection' not found. Configure via User Secrets or environment variable.");

builder.Services.AddDbContext<Library_AppContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<Library_AppContext>();

// Register services
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IAuthorService, AuthorService>();

// Add services to the container with JSON cycle handling
builder.Services.AddControllers().AddJsonOptions(o =>
{
    o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    o.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});
// Enable Swagger/OpenAPI generation and UI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Automatically apply pending migrations (dev convenience) + seed data
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<Library_AppContext>();
    db.Database.Migrate();
    await DbInitializer.SeedAsync(db);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Built-in OpenAPI document endpoint
    app.MapOpenApi();

    // Swashbuckle JSON + UI at /swagger
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Provide simple endpoints for health/ping and root to avoid 404s when "pinging" the server
app.MapGet("/", () => Results.Ok("Server is running"));
app.MapGet("/ping", () => Results.Ok("pong"));

app.Run();
