# ProyectoPruebaViamatica: Gestión de Salas de Cine y Películas

Este proyecto es una implementación de la prueba técnica solicitada, centrada en la gestión de salas de cine y películas. La aplicación proporciona funcionalidades CRUD para ambas entidades y lógica de negocio para la verificación de disponibilidad de salas.

## Tecnologías Utilizadas

* [cite_start]**Backend:** ASP.NET MVC (.NET Framework) [cite: 4]
* **Lenguaje:** C#
* [cite_start]**Base de Datos:** SQL Server [cite: 45]
* **Acceso a Datos:** ADO.NET (SqlConnection, SqlCommand, SqlDataReader)

## Estructura del Proyecto

El proyecto sigue una arquitectura por capas:
* [cite_start]**Controllers:** Contienen la lógica de control para las vistas MVC. [cite: 19, 20]
* [cite_start]**Models:** Definen las entidades del dominio y los ViewModels. [cite: 21, 22]
* [cite_start]**Services:** Contienen la lógica de negocio. [cite: 23, 24]
* [cite_start]**DAL (Data Access Layer) / Repositories:** Contienen la lógica de acceso a la base de datos (ADO.NET). [cite: 25, 26]
* **Views:** Archivos `.cshtml` para la interfaz de usuario.

## Configuración y Ejecución del Proyecto

Sigue estos pasos para poner en marcha el proyecto:

### 1. Configuración de la Base de Datos

El proyecto utiliza SQL Server. Deberás crear la base de datos y sus tablas.

**a. Crear la Base de Datos:**
Abre SQL Server Management Studio (SSMS) o tu herramienta preferida y crea una nueva base de datos. Puedes llamarla `PeliculasDB` o el nombre que prefieras.

**b. Ejecutar Scripts SQL:**
Adjunto en el envío un archivo `database_scripts.sql` (o similar) que contiene los `CREATE TABLE` para las entidades `pelicula`, `sala_cine`, y `pelicula_salacine`, así como `INSERT` de datos de ejemplo y el `CREATE PROCEDURE` para `sp_GetPeliculasByFechaPublicacion`. Ejecuta todo el contenido de este script en tu base de datos recién creada.

**Nota importante:** Asegúrate de que la columna `id_pelicula` en la tabla `pelicula` y `id_sala` en `sala_cine` estén configuradas como `IDENTITY (1,1)` (auto-incremento) en tu base de datos para que las inserciones funcionen correctamente.

### 2. Configuración de la Conexión a la Base de Datos

1.  Abre el proyecto en Visual Studio.
2.  Abre el archivo `Web.config` ubicado en la raíz del proyecto.
3.  Busca la sección `<connectionStrings>`.
4.  Actualiza la cadena de conexión llamada `"CineDBConnection"` con los detalles de tu servidor SQL Server, nombre de base de datos, usuario y contraseña (si aplica).

    ```xml
    <connectionStrings>
        <add name="CineDBConnection"
             connectionString="Data Source=.;Initial Catalog=PeliculasDB;Integrated Security=True;TrustServerCertificate=True"
             providerName="System.Data.SqlClient" />
        </connectionStrings>
    ```

### 3. Ejecutar la Aplicación

1.  En Visual Studio, construye la solución (Build > Build Solution).
2.  Ejecuta la aplicación (Ctrl + F5 o el botón "Start" en la barra de herramientas). Esto iniciará la aplicación en tu navegador predeterminado.

## Funcionalidades Implementadas

### Backend
* **CRUD para Salas de Cine:**
    * Creación de nuevas salas.
    * Listado de salas activas.
    * Actualización de salas existentes.
    * [cite_start]Eliminación lógica de salas (cambio de estado a inactivo). [cite: 39]
* [cite_start]**Procesos de Negocio para Salas de Cine:** [cite: 33]
    * Verificación de disponibilidad de sala por nombre, mostrando mensajes específicos:
        * [cite_start]"Sala disponible" (menos de 3 películas asignadas). [cite: 34, 35]
        * [cite_start]"Sala con [n] películas asignadas" (entre 3 y 5 películas). [cite: 36, 37]
        * [cite_start]"Sala no disponible" (más de 5 películas asignadas). [cite: 38]
        * Mensajes para salas inactivas o no encontradas.
* [cite_start]**CRUD para Películas:** [cite: 29]
    * Funcionalidad de creación de películas implementada.
    * Listado, edición y eliminación están en desarrollo, pero la estructura base ya está establecida.
* [cite_start]**Stored Procedure:** Se utiliza un Stored Procedure (`sp_GetPeliculasByFechaPublicacion`) para obtener películas por fecha de publicación. [cite: 31, 32, 46]

### Frontend (Vistas MVC)
* **Pantalla de Salas de Cine:** Listado, creación, edición y eliminación lógica.
* **Pantalla de Verificación de Disponibilidad de Sala:** Permite buscar una sala por nombre y muestra su estado de disponibilidad.

## Desviaciones de los Requisitos Originales del PDF

Es importante destacar algunas decisiones de implementación que se desviaron de los requisitos estrictos del PDF:

* [cite_start]**Tipo de Proyecto:** El PDF solicitaba una "API-REST" en .NET Core [cite: 4] [cite_start]y un frontend en Angular[cite: 49]. Este proyecto se implementó como una aplicación **ASP.NET MVC 5** (utilizando .NET Framework) con vistas Razor. Esto significa que el frontend de Angular y los endpoints de API-REST puros (para ser probados con Postman/Swagger) no se implementaron.
* [cite_start]**Acceso a Datos:** Se utilizó **ADO.NET** (`SqlConnection`, `SqlCommand`, `SqlDataReader`) para la interacción con la base de datos, en lugar de **Entity Framework** como se especificaba. [cite: 40]

Estas decisiones se tomaron para poder enfocarme en la lógica de negocio y las funcionalidades principales dentro del plazo y con las herramientas disponibles, dado el nivel de experiencia.

## Errores Resueltos o Abordados

* **Error CS0029 (bool a int):** Corregido en `MapearSalaCineDesdeLector` para mapear correctamente el campo `estado` (BIT en DB) a `bool` en el modelo `SalaCine` (o viceversa si el modelo fuera `int`).
* **Error "Cannot insert NULL into id_pelicula":** Este error de base de datos al crear una película se aborda asegurando que la columna `id_pelicula` en la tabla `pelicula` sea `IDENTITY` (auto-incremento). Los scripts SQL proporcionados deberían reflejar esto.
* **Error "Nombre de objeto 'Pelicula_Sala_Cine' no válido":** Corregido el nombre de la tabla en el Stored Procedure `sp_GetPeliculasByFechaPublicacion` a `pelicula_salacine` para que coincida con el esquema de la base de datos.
* **Mensajes de Disponibilidad de Sala:** Ajustada la lógica en `SalasCineController.Disponibilidad` para garantizar que los mensajes específicos de disponibilidad (ej. "Sala disponible", "Sala con N películas asignadas") generados por el repositorio se muestren directamente en la vista.

---