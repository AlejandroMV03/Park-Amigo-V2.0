USE [Estacionamiento]
GO
/****** Object:  Table [dbo].[Registro-usuario]    Script Date: 07/02/2025 09:35:10 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Registro-usuario](
	[Nombre] [varchar](50) NULL,
	[Apellido] [varchar](50) NULL,
	[Nombre_de_usuario] [varchar](50) NULL,
	[Contraseña] [numeric](18, 0) NULL,
	[Numero_de_celular] [numeric](18, 0) NULL
) ON [PRIMARY]
GO
INSERT [dbo].[Registro-usuario] ([Nombre], [Apellido], [Nombre_de_usuario], [Contraseña], [Numero_de_celular]) VALUES (N'Alejandro', N'Mex', N'AlejandroMex1', CAST(123456 AS Numeric(18, 0)), NULL)
GO
