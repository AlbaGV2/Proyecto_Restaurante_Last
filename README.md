# Restaurante Elegance

Este es un proyecto web para un restaurante, desarrollado bajo la arquitectura MVC (Modelo-Vista-Controlador) utilizando el framework **ASP.NET Core** con **.NET 8**.

## 🛠️ Tecnologías Utilizadas

*   **Framework principal:** .NET 8 / ASP.NET Core MVC
*   **Lenguaje:** C#
*   **Acceso a datos:** Entity Framework Core 8
*   **Base de datos:** SQL Server
*   **Frontend:** HTML5, CSS3, Razor Views, Bootstrap

## 📂 Estructura del Proyecto

El proyecto está organizado siguiendo el patrón MVC estándar de ASP.NET Core:

*   **`/Controllers`**: Contiene la lógica principal de la aplicación, gestionando las peticiones del usuario y decidiendo qué vista mostrar. Incluye controladores para Autenticación, Contacto, Inicio, Menú, Quiénes Somos y Reservas.
*   **`/Models`**: Contiene las clases que representan las entidades de la base de datos y la lógica de negocio (por ejemplo, el modelo `Reserva`).
*   **`/Views`**: Contiene los archivos `.cshtml` (Razor) que definen la interfaz gráfica (UI) que verá el usuario final.
*   **`/Data`**: Incluye el contexto de la base de datos (`RestauranteContext.cs`) que conecta los modelos con Entity Framework.
*   **`/Migrations`**: Almacena el historial de cambios estructurales realizados en la base de datos mediante Code-First de Entity Framework.
*   **`/wwwroot`**: Directorio público donde se almacenan los archivos estáticos (CSS, JavaScript, imágenes, iconos, etc.).

## ⚙️ Requisitos Previos

Para ejecutar y modificar este proyecto en un entorno local, necesitas tener instalado:

1.  [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
2.  [SQL Server](https://www.microsoft.com/es-es/sql-server/sql-server-downloads) (Express, Developer Edition) o SQL Server LocalDB.
3.  Un editor de código o IDE compatible, como Visual Studio 2022 o Visual Studio Code (con extensiones de C# y .NET).

## 🚀 Configuración y Ejecución Local

Sigue estos pasos para arrancar el proyecto en tu ordenador:

### 1. Clonar el repositorio
Si aún no lo has hecho, clona el repositorio en tu máquina y sitúate en la carpeta del código fuente:
```bash
git clone <url-del-repositorio>
cd Proyecto_Rest_Last/Proyecto_Web_Restaurante
```

### 2. Configurar la cadena de conexión
En el archivo `appsettings.json`, asegúrate de añadir tu cadena de conexión a la base de datos SQL Server. Debería lucir similar a esto:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=RestauranteEleganceDB;Trusted_Connection=True;MultipleActiveResultSets=true"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "App": {
    "Name": "Restaurante Elegance",
    "Version": "1.0.0",
    "Environment": "Development"
  }
}
```
*(Ajusta el servidor `Server` y el nombre de la base de datos `Database` según la configuración local de tu SQL Server).*

### 3. Aplicar las migraciones de Base de Datos
Abre una terminal en la carpeta del proyecto (`Proyecto_Web_Restaurante`) y ejecuta el siguiente comando para crear las tablas correspondientes en tu base de datos:
```bash
dotnet ef database update
```

### 4. Ejecutar la aplicación
Por último, para iniciar el servidor local, ejecuta:
```bash
dotnet run
```
La terminal te indicará la URL (usualmente `http://localhost:5000` o `https://localhost:5001`) donde podrás abrir y ver la web funcionando en tu navegador.
