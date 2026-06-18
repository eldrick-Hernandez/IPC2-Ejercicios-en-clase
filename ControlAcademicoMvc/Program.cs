var builder = WebApplication.CreateBuilder(args);

// Agregar servicios para usar controladores
builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

// Configurar la ruta por defecto para MVC
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();