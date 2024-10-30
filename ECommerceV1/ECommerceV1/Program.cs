using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ECommerceV1.Data;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<ECommerceV1Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ECommerceV1Context") ?? throw new InvalidOperationException("Connection string 'ECommerceV1Context' not found.")));

//SESSION
builder.Services.AddSession();

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
//to use SESSION
app.UseSession();

app.UseAuthorization();

app.MapRazorPages();
// Définir la page par défaut
app.MapGet("/", async context =>
{
    context.Response.Redirect("/Produits/ListProducts"); // Redirige vers la page par défaut
});

app.Run();
