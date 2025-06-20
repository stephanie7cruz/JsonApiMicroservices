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

### Patrones de diseño usados en el proyecto
1. Inyección de Dependencias (Dependency Injection - DI)
Dónde: En Program.cs al registrar servicios con AddSingleton y al inyectar dependencias en los constructores (ProductClient, IProductService, IInventoryService).

Por qué: Permite desacoplar la creación de objetos de su uso, facilitando la mantenibilidad, testeo unitario (mocking), y escalabilidad del código.

Beneficio: Facilita el reemplazo de implementaciones (por ejemplo, mocks en pruebas unitarias), y hace que el código sea más modular y fácil de extender.

2. Singleton
Dónde: Al registrar servicios con .AddSingleton<IInventoryService, InventoryService>() y similares.

Por qué: Queremos una única instancia del servicio durante toda la vida de la aplicación para gestionar el inventario o productos, evitando estados inconsistentes o cargas innecesarias.

Beneficio: Eficiencia en recursos y estado compartido controlado.

3. Repositorio (Repository)
Dónde: Implícito en el servicio que interactúa con la base de datos (ProductService y InventoryService actúan como repositorios).

Por qué: Se abstrae el acceso a datos en un único lugar, facilitando cambios futuros en la fuente de datos sin afectar la lógica de negocio.

Beneficio: Código más limpio y desacoplado de la base de datos.

4. DTO (Data Transfer Object)
Dónde: Clases como ProductDto usadas para enviar datos estructurados entre servicios.

Por qué: Evita exponer entidades internas directamente y facilita la serialización/deserialización entre servicios.

Beneficio: Control estricto sobre qué datos se comparten y protección contra cambios accidentales.

5. Middleware
Dónde: Implementado como ApiKeyMiddleware para validar las API Keys en los requests.

Por qué: Separar la lógica transversal de autenticación y autorización fuera de los controladores, manteniendo el código más limpio.

Beneficio: Aplicar políticas globales de seguridad y funcionalidad de forma reutilizable y centralizada.

6. Cliente HTTP (HttpClient Factory)
Dónde: Uso de IHttpClientFactory para crear ProductClient.

Por qué: Maneja eficientemente la creación y reuso de clientes HTTP evitando problemas comunes (como agotamiento de sockets).

Beneficio: Código más robusto y optimizado para llamadas HTTP externas

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
