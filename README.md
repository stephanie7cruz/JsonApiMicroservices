# Microservicios ProductService & InventoryService

## Descripción General

Este proyecto implementa dos microservicios independientes en .NET 8:

- **ProductService**: Gestiona los productos con operaciones CRUD.
- **InventoryService**: Gestiona el inventario asociado a los productos y mantiene la cantidad disponible.

Ambos servicios se comunican a través de llamadas HTTP usando API Keys para autenticación. La persistencia de datos se realiza con bases de datos SQL Server locales utilizando Entity Framework Core como ORM.

Se dockerizaron ambos servicios y se orquestan con Docker Compose para facilitar el despliegue local o en ambientes en la nube.

---

## Instrucciones de Instalación y Ejecución

### Requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (local o remoto)
- [Docker y Docker Compose](https://docs.docker.com/get-docker/)

### Configuración

1. Clona el repositorio y navega a la carpeta raíz.

2. Configura las cadenas de conexión SQL Server en `appsettings.json` de cada servicio (`ProductService` e `InventoryService`).

3. Construye y actualiza la base de datos ejecutando migraciones desde cada proyecto:

   ```bash
   cd ProductService
   dotnet ef database update

   cd ../InventoryService
   dotnet ef database update


### Ejecuta pruebas unitarias para validar la lógica

dotnet test
### Levanta los servicios usando Docker Compose:

bash
Copiar

### Accede a los servicios en Swagger UI:

ProductService: http://localhost:5224/swagger/index.html (Dependiendo de tu puerto)

InventoryService: http://localhost:5071/swagger/index.html

Recuerda usar la API Key configurada (super-secreta-productos para ProductService y la correspondiente para InventoryService).

### Arquitectura y Diseño
Microservicios desacoplados: Cada servicio tiene su propia base de datos y lógica de negocio, lo que facilita mantenimiento y escalabilidad.

Comunicación REST con API Keys: Se asegura seguridad básica mediante API Keys en headers HTTP.

Persistencia con Entity Framework Core: Simplifica el manejo de datos y migraciones en SQL Server.

Dockerización y orquestación: Facilita despliegue consistente y replicable localmente o en la nube.

Pruebas unitarias con xUnit: Garantizan estabilidad de la lógica central antes de despliegue.

Swagger para documentación y pruebas interactivas.

### Decisiones Técnicas y Justificaciones
| Decisión                    | Justificación                                                   |
| --------------------------- | --------------------------------------------------------------- |
| .NET 8 como framework       | Última versión estable, rendimiento mejorado y soporte futuro   |
| SQL Server local            | Entorno controlado para desarrollo y pruebas                    |
| Entity Framework Core       | ORM robusto para .NET, facilita migraciones y consultas         |
| API Key para seguridad      | Método sencillo y efectivo para proteger servicios REST         |
| Docker + Docker Compose     | Facilita despliegue y ambiente reproducible                     |
| Arquitectura Microservicios | Escalabilidad, mantenimiento modular y despliegue independiente |
| xUnit para pruebas          | Popular y ampliamente usado para pruebas en .NET                |


### Diagrama de Interacción entre Servicios


+-----------------+                 +------------------+
|                 |   HTTP Request  |                  |
| InventoryService| --------------> |  ProductService  |
|                 |   (GET producto)|                  |
+-----------------+                 +------------------+
       |                                   |
       | Consulta detalles del producto    |
       |                                   |
       | <---------------------------------|
       |          Respuesta JSON           |
       |                                   |
       | Devuelve info combinada al cliente|
       +-----------------------------------+

Cliente (Postman, Frontend)
    |
    |--> ProductService (CRUD productos)
    |--> InventoryService (Consulta/actualiza inventario)

### Cómo funcionan los endpoints principales

GET /api/products/{id}: Obtiene detalles de un producto.

GET /api/inventories/{productoId}: Obtiene cantidad disponible de un producto en inventario.

PUT /api/inventories/{productoId}: Actualiza cantidad en inventario tras compra, pasando la cantidad comprada en el body como entero.

GET /api/inventories/{productoId}/detalle: Devuelve información combinada del producto y su inventario, consultando ambos servicios.

### Uso de SQL Server y Entity Framework Core
Cada microservicio maneja su propia base de datos SQL Server para mantener independencia y escalabilidad.

Se utiliza Entity Framework Core para modelar entidades, manejar migraciones y consultar/actualizar datos.

Migraciones se aplican mediante dotnet ef database update.

Las claves primarias se generan con IDENTITY para autoincremento.

#### Pruebas Unitarias
Se implementaron pruebas unitarias con xUnit para validar la lógica de negocio en ambos servicios.

Pruebas incluyen casos para obtener productos, actualizar inventarios y validar reglas de negocio.

Se ejecutan con dotnet test.

Permiten detectar errores temprano y asegurar calidad del código.

#### Docker y Orquestación con Docker Compose
Se dockerizó cada microservicio con su propio Dockerfile.

Docker Compose orquesta la construcción y despliegue de ambos servicios juntos.

Los puertos configurados en Compose son:

ProductService: 5224

InventoryService: 5071

El entorno está configurado para desarrollo (ASPNETCORE_ENVIRONMENT=Development).

Para levantar el entorno, solo ejecutar:
