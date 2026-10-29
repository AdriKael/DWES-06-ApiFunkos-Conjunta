# ApiFunkosConjunta

API REST de gestión de Funkos (figuras de vinilo coleccionables), desarrollada en .NET 10.

## Alcance

- **Alcance base (práctica 3, fases 1 a 8):** diseño REST, CRUD con controladores, inyección de
  dependencias, patrón Result, DTOs/validaciones/mappers, configuración y logs (Serilog).
- **Extras incluidos (fases 9 a 12):** persistencia real en PostgreSQL, caché en Redis,
  documentación con Swagger/OpenAPI, tests unitarios e integración con Testcontainers y
  despliegue con Docker Compose.

Los extras son opcionales: la API puede ejecutarse en memoria desactivando PostgreSQL/Redis,
aunque por defecto se usan para aprovechar el despliegue completo.

## Tecnologías

- ASP.NET Core 10 (controladores MVC)
- Entity Framework Core + PostgreSQL
- Redis (caché distribuida)
- Serilog (consola + fichero)
- CSharpFunctionalExtensions (patrón Result)
- NUnit + FluentAssertions + Testcontainers
- Docker / Docker Compose

## Ejecución local

Requisitos: .NET 10 SDK, PostgreSQL y Redis en ejecución.

```bash
cd ApiFunkosConjunta
dotnet run --project ApiFunkosConjunta/ApiFunkosConjunta.csproj
```

La API queda disponible en `https://localhost:5001` / `http://localhost:5000`.
Swagger UI: `http://localhost:5000/swagger`.

## Ejecución con Docker

```bash
cd ApiFunkosConjunta
docker compose up -d
```

Servicios:
- API: http://localhost:8082
- Swagger: http://localhost:8082/swagger
- PostgreSQL: localhost:5432
- Redis: localhost:6379

## Tests

```bash
cd ApiFunkosConjunta
dotnet test ApiFunkosConjunta.Tests/ApiFunkosConjunta.Tests.csproj
```

Incluye tests unitarios del servicio y tests de integración con Testcontainers (PostgreSQL).

## Endpoints principales

| Operación | Verbo | Ruta | Éxito |
|-----------|-------|------|-------|
| Listar todos los Funkos | GET | /api/funkos | 200 |
| Consultar un Funko por ID | GET | /api/funkos/{id} | 200 |
| Crear un Funko | POST | /api/funkos | 201 |
| Actualizar un Funko | PUT | /api/funkos/{id} | 200 |
| Eliminar un Funko | DELETE | /api/funkos/{id} | 204 |
| Buscar Funkos por nombre | GET | /api/funkos/search?name= | 200 |

## Justificación de decisiones

- **Controladores MVC**: elegido sobre Minimal APIs por mayor estructura y testabilidad.
- **PostgreSQL**: base de datos relacional robusta, con soporte EF Core.
- **Redis**: caché distribuida compartida entre instancias.
- **Patrón Result**: evita excepciones para errores de dominio y devuelve Problem Details.
- **Soft-delete**: los Funkos y categorías se marcan como eliminados, no se borran físicamente.
