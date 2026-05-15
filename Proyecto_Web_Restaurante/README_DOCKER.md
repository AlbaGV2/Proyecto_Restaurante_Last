# Docker Setup for Restaurante Project

This setup provides a complete environment running both the ASP.NET Core Web Application and a SQL Server 2022 container with the database and schema pre-initialized.

## How to use

1.  **Start the environment**:
    Run the following command in this directory to build the web image and start both the database and the web application in the background:
    ```bash
    docker-compose up -d --build
    ```

2.  **Access the Application**:
    Once the containers are running, you can access the web application at:
    `http://localhost:5001`

3.  **Connection String (for local development outside Docker)**:
    If you still want to run the web app outside of Docker (using `dotnet run` or from Visual Studio) while keeping the database in Docker, ensure your `appsettings.json` points to:
    ```json
    "ConnectionStrings": {
      "DefaultConnection": "Server=localhost,1433;Database=RestauranteBD;User Id=sa;Password=YourStrong@Password123;TrustServerCertificate=True;"
    }
    ```
    *Note: When running inside Docker, the web container automatically uses `Server=db` to connect to the database, which is configured via environment variables in `docker-compose.yml`.*

## Files
- `docker-compose.yml`: Defines the `web` application service and the `db` (SQL Server) service.
- `Dockerfile`: Multi-stage build instructions to compile and run the ASP.NET Core web application.
- `sql/init.sql`: Script to create the database and tables on startup for the SQL Server container.
