# MultiTenantApp

## Descripción

MultiTenantApp es una aplicación diseñada para soportar múltiples organizaciones (tenants) utilizando la arquitectura de multitenancy. La aplicación incluye funcionalidades de autenticación y autorización mediante JWT, gestión de productos para cada tenant, y una arquitectura limpia utilizando .NET Core.

## Estructura del Proyecto

El proyecto está dividido en varias capas siguiendo los principios de Clean Architecture:

- **MultiTenantApp.API**: Capa de presentación que expone los endpoints REST.
- **MultiTenantApp.Application**: Contiene la lógica de aplicación y los manejadores de comandos/consultas.
- **MultiTenantApp.Domain**: Contiene las entidades del dominio y las interfaces.
- **MultiTenantApp.Infrastructure**: Implementaciones de persistencia y otros servicios.

## Configuración

### Prerrequisitos

- .NET 6 SDK
- SQL Server

### Configuración del entorno

1. Clona el repositorio:
    ```bash
    git clone https://github.com/rcarrascof/MultiTenantApp.git
    cd MultiTenantApp
    ```

2. Configura las cadenas de conexión en `appsettings.json` en el proyecto `MultiTenantApp.API`:
    ```json
    {
      "Logging": {
        "LogLevel": {
          "Default": "Information",
          "Microsoft.AspNetCore": "Warning"
        }
      },
      "AllowedHosts": "*",
      "ConnectionStrings": {
        "OrgUsersDb": "Server=RCARRASCO-LAP;Database=OrgUsersDb;User Id=sa;Password=wladimir;",
        "ProductsDb": "Server=RCARRASCO-LAP\\MSSQLSERVER02;Database={0}_ProductsDb;User Id=sa;Password=wladimir;"
      },
      "Jwt": {
        "Key": "your_secret_key",
        "Issuer": "your_issuer",
        "Audience": "your_audience"
      }
    }
    ```

3. Restaura las dependencias e inicializa las migraciones:
    ```bash
    dotnet restore
    dotnet ef migrations add InitialCreate -c OrgUsersDbContext -p ../MultiTenantApp.Infrastructure -s ../MultiTenantApp.API
    dotnet ef database update -c OrgUsersDbContext -p ../MultiTenantApp.Infrastructure -s ../MultiTenantApp.API

    dotnet ef migrations add InitialCreate -c ProductsDbContext -p ../MultiTenantApp.Infrastructure -s ../MultiTenantApp.API
    ```

## Ejecución

Para ejecutar la aplicación, utiliza el siguiente comando desde el directorio del proyecto `MultiTenantApp.API`:
```bash
dotnet run

La API estará disponible en `https://localhost:7213`.

## Endpoints

### Crear una Organización y un Usuario

Para crear una organización y un usuario inicial, puedes utilizar el siguiente endpoint:

- **Crear Organización y Usuario**
  - **POST** `/api/organizations`
  - **Headers**:
    - `Content-Type`: `application/json`
  - **Body**:
    ```json
    {
      "OrganizationName": "New Organization",
      "Slug": "new_org",
      "Username": "newuser",
      "Password": "newpassword"
    }
    ```
  - **Response**:
    ```json
    {
      "message": "Organization created successfully",
      "organization": {
        "id": 1,
        "name": "New Organization",
        "slug": "new_org"
      }
    }
    ```

### Autenticación

- **Login**
  - **POST** `/api/auth/login`
  - **Body**:
    ```json
    {
        "Username": "newuser",
        "Password": "newpassword"
    }
    ```
  - **Response**:
    ```json
    {
        "token": "jwt_token",
        "user": {
            "id": 1,
            "username": "newuser",
            "organizationId": 1,
            "organizationName": "New Organization"
        }
    }
    ```

### Gestión de Productos

- **Obtener Producto por ID**
  - **GET** `/api/{slugTenant}/products/{id}`
  - **Headers**: `Authorization: Bearer {jwt_token}`
  - **Response**:
    ```json
    {
        "id": 1,
        "name": "Product Name",
        "price": 100.00
    }
    ```

- **Obtener Lista de Productos**
  - **GET** `/api/{slugTenant}/products`
  - **Headers**: `Authorization: Bearer {jwt_token}`
  - **Response**:
    ```json
    [
        {
            "id": 1,
            "name": "Product Name",
            "price": 100.00
        }
    ]
    ```

- **Crear Producto**
  - **POST** `/api/{slugTenant}/products`
  - **Headers**: `Authorization: Bearer {jwt_token}`
  - **Body**:
    ```json
    {
        "Name": "New Product",
        "Price": 100.00
    }
    ```

- **Actualizar Producto**
  - **PUT** `/api/{slugTenant}/products/{id}`
  - **Headers**: `Authorization: Bearer {jwt_token}`
  - **Body**:
    ```json
    {
        "Id": 1,
        "Name": "Updated Product",
        "Price": 150.00
    }
    ```

- **Eliminar Producto**
  - **DELETE** `/api/{slugTenant}/products/{id}`
  - **Headers**: `Authorization: Bearer {jwt_token}`



## Licencia

Este proyecto está licenciado bajo la Licencia MIT. Para más información, consulta el archivo [LICENSE](LICENSE).

