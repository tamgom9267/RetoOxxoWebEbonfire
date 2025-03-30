using RetoOxxoWeb.Model; // Carpeta que contiene la clase DataBaseContext
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios a la aplicación
builder.Services.AddRazorPages();
builder.Services.AddDistributedMemoryCache();


// Habilitar sesiones
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddHttpContextAccessor();

builder.Services.Add(new ServiceDescriptor(typeof(DataBaseContext), new DataBaseContext()));

var app = builder.Build();

// Configurar el middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Usar sesiones
app.UseSession();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.MapGet("/", async context =>
{
    context.Response.Redirect("/IniciarSesion");
});

app.Run();
