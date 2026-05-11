# SQL Server Docker Setup for Restaurante Project

This setup provides a SQL Server 2022 container with the database and schema pre-initialized.

## How to use

1.  **Start the database**:
    Run the following command in this directory:
    ```bash
    docker-compose up -d
    ```

2.  **Connection String**:
    Update your `appsettings.json` (or `appsettings.Development.json`) to point to the Docker container:
    ```json
    "ConnectionStrings": {
      "DefaultConnection": "Server=localhost,1433;Database=RestauranteBD;User Id=sa;Password=YourStrong@Password123;TrustServerCertificate=True;"
    }
    ```

## Files
- `docker-compose.yml`: Defines the SQL Server service and volume.
- `sql/init.sql`: Script to create the database and tables on startup.
