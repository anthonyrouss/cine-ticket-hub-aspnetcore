using System.Configuration;
using CineTicketHub.Enums;
using CineTicketHub.Mappers;
using CineTicketHub.Models;
using CineTicketHub.Models.Entities;
using CineTicketHub.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var serverVersion = new MySqlServerVersion(new Version(8, 0, 35));

builder.Services.AddDbContext<CineTicketHubContext>(options => 
    options.UseMySql(connectionString, serverVersion));

builder.Services.AddAuthorization();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => 
        options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<CineTicketHubContext>()
    .AddDefaultTokenProviders();
    

// Services config
builder.Services.AddScoped<IMoviesService, MoviesService>();
builder.Services.AddScoped<IGenresService, GenresService>();
builder.Services.AddScoped<MovieMapper>();

// Identity
builder.Services.AddRazorPages();
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AddAreaPageRoute("Identity", "/Account/Login", "/Account/Login");
    options.Conventions.AddAreaPageRoute("Identity", "/Account/AccessDenied", "/Account/AccessDenied");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    // Seed roles
    foreach (var role in Enum.GetNames(typeof(UserRole)))
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }
}

using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    // Dev: Create admin account if not exists
    string adminEmail = "admin@dev.com";
    string adminPassword = "Admin1!";

    if (await userManager.FindByEmailAsync(adminEmail) == null)
    {
        var adminUser = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail
        };

        await userManager.CreateAsync(adminUser, adminPassword);

        await userManager.AddToRoleAsync(adminUser, UserRole.ADMIN.ToString());
    }

    // Dev: Create content manager account if not exists
    string contentManagerEmail = "cmanager@dev.com";
    string contentManagerPassword = "Cmanager1!";

    if (await userManager.FindByEmailAsync(contentManagerEmail) == null)
    {
        var contentManagerUser = new ApplicationUser
        {
            UserName = contentManagerEmail,
            Email = contentManagerEmail
        };

        await userManager.CreateAsync(contentManagerUser, contentManagerPassword);

        await userManager.AddToRoleAsync(contentManagerUser, UserRole.CONTENT_MANAGER.ToString());
    }
}


app.Run();