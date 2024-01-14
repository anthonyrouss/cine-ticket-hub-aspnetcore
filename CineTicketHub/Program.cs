using System.Configuration;
using CineTicketHub.Mappers;
using CineTicketHub.Models;
using CineTicketHub.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var serverVersion = new MySqlServerVersion(new Version(8, 0, 35));

builder.Services.AddDbContext<CineTicketHubContext>(options => 
    options.UseMySql(connectionString, serverVersion));

// Services config

builder.Services.AddScoped<IMoviesService, MoviesService>();
builder.Services.AddScoped<IGenresService, GenresService>();
builder.Services.AddScoped<MovieMapper>();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();