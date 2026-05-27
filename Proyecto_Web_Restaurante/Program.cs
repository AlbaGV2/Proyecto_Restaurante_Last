using Microsoft.EntityFrameworkCore;
using Restaurante.Data; // Importa nuestra nueva carpeta Data
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Añadir servicios MVC al contenedor
builder.Services.AddControllersWithViews();

// Conecta el DbContext con la cadena de tu appsettings.json
builder.Services.AddDbContext<RestauranteContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configurar Antiforgery para que acepte el token desde la cabecera (Header) para peticiones AJAX/JSON
builder.Services.AddAntiforgery(options => options.HeaderName = "RequestVerificationToken");

// Configurar Autenticación por Cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.LogoutPath = "/Auth/Logout";
        options.AccessDeniedPath = "/Auth/Login";
    });

var app = builder.Build();

// Configurar el pipeline HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
    app.UseHttpsRedirection();
}

// Usa la carpeta wwwroot para archivos estáticos por defecto
app.UseStaticFiles(); 

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Configurar el enrutamiento predeterminado de MVC
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Aplicar migraciones automáticamente al arrancar (útil para Docker)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<RestauranteContext>();
        int retries = 10;
        while (retries > 0)
        {
            try
            {
                app.Logger.LogInformation("Intentando aplicar migraciones en la base de datos...");
                context.Database.Migrate();
                app.Logger.LogInformation("¡Migraciones aplicadas con éxito!");
                break;
            }
            catch (Exception ex)
            {
                retries--;
                app.Logger.LogWarning($"Base de datos no disponible aún. Reintentando en 5 segundos... ({retries} intentos restantes). Error: {ex.Message}");
                System.Threading.Thread.Sleep(5000);
                if (retries == 0)
                {
                    throw;
                }
            }
        }
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "Ocurrió un error al aplicar las migraciones de la base de datos.");
    }
}

app.Run();
