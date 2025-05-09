USE [master]
GO
/****** Object:  Database [Estacionamiento]    Script Date: 11/04/2025 02:59:32 p. m. ******/
CREATE DATABASE [Estacionamiento]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Estacionamiento', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\Estacionamiento.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Estacionamiento_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\Estacionamiento_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [Estacionamiento] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Estacionamiento].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Estacionamiento] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Estacionamiento] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Estacionamiento] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Estacionamiento] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Estacionamiento] SET ARITHABORT OFF 
GO
ALTER DATABASE [Estacionamiento] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Estacionamiento] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Estacionamiento] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Estacionamiento] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Estacionamiento] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Estacionamiento] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Estacionamiento] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Estacionamiento] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Estacionamiento] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Estacionamiento] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Estacionamiento] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Estacionamiento] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Estacionamiento] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Estacionamiento] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Estacionamiento] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Estacionamiento] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Estacionamiento] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Estacionamiento] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [Estacionamiento] SET  MULTI_USER 
GO
ALTER DATABASE [Estacionamiento] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Estacionamiento] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Estacionamiento] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Estacionamiento] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Estacionamiento] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [Estacionamiento] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [Estacionamiento] SET QUERY_STORE = ON
GO
ALTER DATABASE [Estacionamiento] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [Estacionamiento]
GO
/****** Object:  Table [dbo].[Administrador]    Script Date: 11/04/2025 02:59:32 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Administrador](
	[IdAdministrador] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](50) NULL,
	[Apellido] [nvarchar](50) NULL,
	[NombreDeAdministrador] [nvarchar](50) NULL,
	[Contraseña] [nvarchar](50) NULL,
	[Tarjeta] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[IdAdministrador] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Registro_Usuario]    Script Date: 11/04/2025 02:59:32 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Registro_Usuario](
	[ID_Registro] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](50) NOT NULL,
	[Apellido] [varchar](50) NOT NULL,
	[Nombre_de_usuario] [varchar](50) NOT NULL,
	[Contraseña] [varchar](255) NOT NULL,
	[Numero_de_celular] [varchar](15) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID_Registro] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Nombre_de_usuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Numero_de_celular] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RegistrodeEstacionamiento]    Script Date: 11/04/2025 02:59:32 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RegistrodeEstacionamiento](
	[ID_Estacionamiento] [int] IDENTITY(1,1) NOT NULL,
	[FK_Registro] [int] NOT NULL,
	[FK_Tarjetas] [int] NOT NULL,
	[FK_Reservas] [int] NOT NULL,
	[HoraEntrada] [datetime] NOT NULL,
	[HoraSalida] [datetime] NULL,
	[Pago] [decimal](10, 2) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID_Estacionamiento] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Reservas]    Script Date: 11/04/2025 02:59:32 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Reservas](
	[ID_Reservas] [int] IDENTITY(1,1) NOT NULL,
	[FK_Registro] [int] NOT NULL,
	[FK_Tarjetas] [int] NULL,
	[NombreUsuario] [varchar](50) NOT NULL,
	[FechaReserva] [date] NOT NULL,
	[HoraReserva] [time](7) NOT NULL,
	[Lugar] [varchar](50) NOT NULL,
	[Estatus] [varchar](20) NOT NULL,
	[Numero_Tarjetas] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID_Reservas] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Solicitudes]    Script Date: 11/04/2025 02:59:32 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Solicitudes](
	[ID_Solicitudes] [int] IDENTITY(1,1) NOT NULL,
	[NombreUsuario] [nvarchar](100) NOT NULL,
	[FechaSolicitud] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID_Solicitudes] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tarjetas]    Script Date: 11/04/2025 02:59:32 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tarjetas](
	[ID_Tarjetas] [int] IDENTITY(1,1) NOT NULL,
	[FK_Registro] [int] NOT NULL,
	[Numero_Tarjeta] [varchar](20) NOT NULL,
	[Credito] [decimal](10, 2) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID_Tarjetas] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Numero_Tarjeta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[RegistrodeEstacionamiento]  WITH CHECK ADD  CONSTRAINT [FK_Estacionamiento_Registro] FOREIGN KEY([FK_Registro])
REFERENCES [dbo].[Registro_Usuario] ([ID_Registro])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RegistrodeEstacionamiento] CHECK CONSTRAINT [FK_Estacionamiento_Registro]
GO
ALTER TABLE [dbo].[RegistrodeEstacionamiento]  WITH CHECK ADD  CONSTRAINT [FK_Estacionamiento_Reservas] FOREIGN KEY([FK_Reservas])
REFERENCES [dbo].[Reservas] ([ID_Reservas])
GO
ALTER TABLE [dbo].[RegistrodeEstacionamiento] CHECK CONSTRAINT [FK_Estacionamiento_Reservas]
GO
ALTER TABLE [dbo].[RegistrodeEstacionamiento]  WITH CHECK ADD  CONSTRAINT [FK_Estacionamiento_Tarjetas] FOREIGN KEY([FK_Tarjetas])
REFERENCES [dbo].[Tarjetas] ([ID_Tarjetas])
GO
ALTER TABLE [dbo].[RegistrodeEstacionamiento] CHECK CONSTRAINT [FK_Estacionamiento_Tarjetas]
GO
ALTER TABLE [dbo].[Reservas]  WITH CHECK ADD  CONSTRAINT [FK_Reservas_Registro] FOREIGN KEY([FK_Registro])
REFERENCES [dbo].[Registro_Usuario] ([ID_Registro])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Reservas] CHECK CONSTRAINT [FK_Reservas_Registro]
GO
ALTER TABLE [dbo].[Reservas]  WITH CHECK ADD  CONSTRAINT [FK_Reservas_Tarjetas] FOREIGN KEY([FK_Tarjetas])
REFERENCES [dbo].[Tarjetas] ([ID_Tarjetas])
GO
ALTER TABLE [dbo].[Reservas] CHECK CONSTRAINT [FK_Reservas_Tarjetas]
GO
ALTER TABLE [dbo].[Tarjetas]  WITH CHECK ADD  CONSTRAINT [FK_Tarjetas_Registro] FOREIGN KEY([FK_Registro])
REFERENCES [dbo].[Registro_Usuario] ([ID_Registro])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Tarjetas] CHECK CONSTRAINT [FK_Tarjetas_Registro]
GO
USE [master]
GO
ALTER DATABASE [Estacionamiento] SET  READ_WRITE 
GO
