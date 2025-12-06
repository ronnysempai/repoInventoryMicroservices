# Inventory Microservices (.NET)

## Requisitos
- .NET 7 (o .NET 8)
- Docker & Docker Compose
- SQL Server (si no usa Docker)

## Estructura
- src/ProductService: microservicio de productos (API)
- src/TransactionService: microservicio de transacciones
- SharedKernel: modelos compartidos
- scripts/init-db.sql: script inicial de BD

## Ejecutar con Docker (desarrollo)
1. Edita contraseñas en `docker-compose.yml` si lo deseas.
2. `docker-compose up --build`
3. ProductService estará en http://localhost:5079
4. TransactionService estará en http://localhost:5037

## Ejecutar local (sin docker)
- Crear DB con `scripts/init-db.sql`
- Abrir solución `inventory.sln`
- Configurar `appsettings.json` con la cadena de conexión
- Ejecutar migraciones:
  - `dotnet ef migrations add Init --project src/ProductService --startup-project src/ProductService`
  - `dotnet ef database update --project src/ProductService --startup-project src/ProductService`
  - Repetir para TransactionService

## Endpoints principales
- `GET /api/products`
- `POST /api/products`
- `PUT /api/products/{id}`
- `DELETE /api/products/{id}`

- `GET /api/transactions`
- `POST /api/transactions` (body: tipo "compra" o "venta", productId, quantity, unitPrice)

## Observaciones
- Al crear una transacción tipo `venta`, el servicio valida stock y luego actualiza stock en ProductService.
- En producción usar mensajería (event-driven) para eventual consistencia. Para la prueba, la comunicación sincrónica cumple los requisitos.
