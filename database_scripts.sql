USE [PeliculasDB]
GO
/****** Object:  Table [dbo].[pelicula]    Script Date: 08/07/2025 17:13:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[pelicula](
	[id_pelicula] [int] IDENTITY(1,1) NOT NULL,
	[nombre] [nchar](100) NOT NULL,
	[duracion] [int] NOT NULL,
	[Activo] [bit] NOT NULL,
 CONSTRAINT [UQ_Pelicula_id_pelicula] UNIQUE NONCLUSTERED 
(
	[id_pelicula] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[pelicula_salacine]    Script Date: 08/07/2025 17:13:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[pelicula_salacine](
	[id_pelicula_sala] [int] IDENTITY(1,1) NOT NULL,
	[id_sala_cine] [int] NULL,
	[fecha_publicacion] [date] NULL,
	[fecha_fin] [date] NULL,
	[id_pelicula] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[sala_cine]    Script Date: 08/07/2025 17:13:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sala_cine](
	[id_sala] [int] IDENTITY(1,1) NOT NULL,
	[nombre] [nchar](100) NULL,
	[estado] [bit] NOT NULL,
 CONSTRAINT [UQ_SalaCine_id_sala] UNIQUE NONCLUSTERED 
(
	[id_sala] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[pelicula_salacine]  WITH CHECK ADD  CONSTRAINT [FK_PeliculaSalaCine_Pelicula] FOREIGN KEY([id_pelicula])
REFERENCES [dbo].[pelicula] ([id_pelicula])
GO
ALTER TABLE [dbo].[pelicula_salacine] CHECK CONSTRAINT [FK_PeliculaSalaCine_Pelicula]
GO
ALTER TABLE [dbo].[pelicula_salacine]  WITH CHECK ADD  CONSTRAINT [FK_PeliculaSalaCine_SalaCine] FOREIGN KEY([id_sala_cine])
REFERENCES [dbo].[sala_cine] ([id_sala])
GO
ALTER TABLE [dbo].[pelicula_salacine] CHECK CONSTRAINT [FK_PeliculaSalaCine_SalaCine]
GO
/****** Object:  StoredProcedure [dbo].[sp_GetPeliculasByFechaPublicacion]    Script Date: 08/07/2025 17:13:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Crea el procedimiento almacenado
CREATE PROCEDURE [dbo].[sp_GetPeliculasByFechaPublicacion]
    @fecha DATE -- Nombre del parámetro en minúsculas para que coincida con tu código C#
AS
BEGIN
    SET NOCOUNT ON;

    SELECT DISTINCT
        P.id_pelicula,
        P.nombre,
        P.duracion,
        P.Activo
    FROM
        Pelicula P -- Nombre de la tabla "Pelicula"
    INNER JOIN
        pelicula_salacine PSC ON P.id_pelicula = PSC.id_pelicula -- Nombre de la tabla "pelicula_salacine" en minúsculas
    WHERE
        PSC.fecha_publicacion = @fecha -- Filtra por la fecha de publicación
        AND P.Activo = 1; -- Asumo que quieres solo películas activas
END;
GO
/****** Object:  StoredProcedure [dbo].[usp_BuscarPeliculaPorNombre]    Script Date: 08/07/2025 17:13:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--sirve para buscar pelicula por nombre
CREATE PROCEDURE [dbo].[usp_BuscarPeliculaPorNombre]
	@NombrePelicula NVARCHAR(255)
AS
BEGIN
	SET NOCOUNT ON;

    SELECT
        id_pelicula,
        nombre,
        duracion,
        Activo
    FROM
        pelicula
    WHERE
        nombre LIKE '%' + @NombrePelicula + '%'
    ORDER BY
        nombre;
END
GO
