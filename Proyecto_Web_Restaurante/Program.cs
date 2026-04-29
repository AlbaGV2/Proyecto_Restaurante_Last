using Microsoft.EntityFrameworkCore;
using Restaurante.Data; // Importa nuestra nueva carpeta Data

var builder = WebApplication.CreateBuilder(args);

// Añadir servicios MVC al contenedor
builder.Services.AddControllersWithViews();

// Conecta el DbContext con la cadena de tu appsettings.json
builder.Services.AddDbContext<RestauranteContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configurar el pipeline HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// Usa la carpeta wwwroot para archivos estáticos por defecto
app.UseStaticFiles(); 

app.UseRouting();

app.UseAuthorization();

// Configurar el enrutamiento predeterminado de MVC
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
