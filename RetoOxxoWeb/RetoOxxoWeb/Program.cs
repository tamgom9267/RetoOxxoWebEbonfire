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

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


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
app.UseCors();

// Usar sesiones
app.UseSession();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.MapGet("/", async context =>
{
    var userId = context.Session.GetInt32("usuarioID");
    if (userId == null)
    {
        context.Response.Redirect("/IniciarSesion");
    }
    else
    {
        context.Response.Redirect("/Index");
    }
});

app.Run();
