IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'RestauranteBD')
BEGIN
    CREATE DATABASE RestauranteBD;
END
GO

USE RestauranteBD;
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Reservas]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Reservas] (
        [Id] int IDENTITY(1,1) NOT NULL,
        [Nombre] nvarchar(30) NOT NULL,
        [Email] nvarchar(50) NOT NULL,
        [Fecha] datetime2 NOT NULL,
        [Hora] nvarchar(max) NOT NULL,
        [Personas] int NOT NULL,
        [Turno] nvarchar(max) NOT NULL,
        [IdReserva] nvarchar(max) NOT NULL,
        [FechaCreacion] datetime2 NOT NULL,
        [UltimaModificacion] datetime2 NULL,
        [IsDeleted] bit NOT NULL DEFAULT 0,
        [FechaEliminacion] datetime2 NULL,
        [WasRestored] bit NOT NULL DEFAULT 0,
        CONSTRAINT [PK_Reservas] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LogDescargas]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[LogDescargas] (
        [Id] int IDENTITY(1,1) NOT NULL,
        [UsuarioNombre] nvarchar(max) NOT NULL,
        [UsuarioId] int NULL,
        [FechaDescarga] datetime2 NOT NULL,
        [Formato] nvarchar(max) NOT NULL,
        [TotalRegistros] int NOT NULL,
        [IpAddress] nvarchar(max) NOT NULL,
        [FiltroFechaInicio] datetime2 NULL,
        [FiltroFechaFin] datetime2 NULL,
        CONSTRAINT [PK_LogDescargas] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
END
GO
