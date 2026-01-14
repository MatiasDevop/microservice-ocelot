# Microservices Solution - Local Setup Guide

## Overview
This solution contains 4 microservices orchestrated with Docker Compose:
- **ProductMicroservice** - Product management API (Port 4201)
- **UserMicroservice** - User management API (Port 4202)
- **CachingApi** - Caching layer with Redis and PostgreSQL (Port 4203)
- **OcelotApiGateway** - API Gateway for routing (Port 7130)

## Prerequisites
- Docker Desktop for Windows
- .NET 8 SDK (for local development)
- SQL Server Management Studio (optional, for DB management)

## Quick Start

### 1. Start All Services with Docker Compose

```powershell
docker compose up --build
```

This will:
- Create SQL Server, PostgreSQL, and Redis containers
- Build and start all 4 microservices
- Apply health checks and dependency ordering

### 2. Apply Database Migrations

After containers are running, apply EF Core migrations:

```powershell
# Product Microservice (SQL Server)
docker compose exec productservice dotnet ef database update

# User Microservice (SQL Server)
docker compose exec userservice dotnet ef database update

# Caching API (PostgreSQL)
docker compose exec cachingapi dotnet ef database update
```

### 3. Verify Services

Access Swagger UIs:
- Product API: http://localhost:4201/swagger
- User API: http://localhost:4202/swagger
- Caching API: http://localhost:4203/swagger
- Ocelot Gateway: http://localhost:7130/swagger

## API Gateway Routes

Access services through Ocelot at http://localhost:7130:

### Products
- `GET /gateway/product` - List all products
- `GET /gateway/product/{id}` - Get product by ID
- `POST /gateway/product` - Create product
- `PUT /gateway/product` - Update product
- `DELETE /gateway/product/{id}` - Delete product

### Users
- `GET /gateway/user` - List all users
- `GET /gateway/user/{id}` - Get user by ID
- `POST /gateway/user` - Create user
- `PUT /gateway/user` - Update user
- `DELETE /gateway/user/{id}` - Delete user

### Drivers (Cached)
- `GET /gateway/drivers` - List all drivers (cached)
- `POST /gateway/drivers` - Add driver
- `DELETE /gateway/drivers` - Delete driver

## Configuration

### Connection Strings

All connection strings are configured in `docker-compose.yml`:
- **SQL Server**: `Server=sqlserver;Database={DB};User Id=sa;Password=YourStrong!Passw0rd`
- **PostgreSQL**: `Server=postgres;Port=5432;Database=Sampledb;User ID=user_demo;Password=12345678`
- **Redis**: `Host=redis;Port=6379`

### Environment Variables

Each service uses:
- `ASPNETCORE_ENVIRONMENT=Development`
- `ASPNETCORE_HTTP_PORTS=8080`

## Local Development (Without Docker)

### 1. Install Dependencies
- SQL Server 2022 (or SQL Server Express)
- PostgreSQL 15
- Redis 7

### 2. Update Connection Strings

Edit `appsettings.json` in each project to use `localhost`:

**ProductMicroservice/appsettings.json:**
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=ProductServiceDB;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=true"
}
```

**UserMicroservice/appsettings.json:**
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=UserServiceDB;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=true"
}
```

**CachingApi/appsettings.json:**
```json
"ConnectionStrings": {
  "SampleDbConnection": "Server=localhost;Port=5432;Database=Sampledb;User ID=user_demo;Password=12345678"
},
"Redis": {
  "Host": "localhost",
  "Port": "6379"
}
```

**OcelotApiGateway/ocelot.json:**
Update `DownstreamHostAndPorts` to use `localhost` and appropriate ports (4201, 4202, 4203).

### 3. Apply Migrations

```powershell
dotnet ef database update --project ProductMicroservice/ProductMicroservice.csproj
dotnet ef database update --project UserMicroservice/UserMicroservice.csproj
dotnet ef database update --project CachingApi/CachingApi.csproj
```

### 4. Run Services

```powershell
# Terminal 1
cd ProductMicroservice
dotnet run

# Terminal 2
cd UserMicroservice
dotnet run

# Terminal 3
cd CachingApi
dotnet run

# Terminal 4
cd OcelotApiGateway
dotnet run
```

## Troubleshooting

### Migrations Failing
If migrations fail, ensure databases are ready:
```powershell
# Check SQL Server is accepting connections
docker compose exec sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P YourStrong!Passw0rd -Q "SELECT @@VERSION"

# Check PostgreSQL
docker compose exec postgres psql -U user_demo -d Sampledb -c "SELECT version();"

# Check Redis
docker compose exec redis redis-cli ping
```

### Port Conflicts
If ports are already in use, modify the left side of port mappings in `docker-compose.yml`:
```yaml
ports:
  - "14201:8080"  # Change 4201 to 14201
```

### Containers Not Starting
View logs:
```powershell
docker compose logs productservice
docker compose logs userservice
docker compose logs cachingapi
docker compose logs ocelot
```

## Cleanup

Stop and remove all containers:
```powershell
docker compose down
```

Remove volumes (deletes all data):
```powershell
docker compose down -v
```

## Next Steps

### Recommended Improvements
1. **Add async/await** to service methods
2. **Implement DTOs** instead of exposing EF entities
3. **Add validation** with FluentValidation
4. **Add health checks** (`/health` endpoints)
5. **Implement logging** with Serilog
6. **Add authentication** with JWT tokens
7. **Restrict CORS** in production
8. **Add unit tests** with xUnit/NUnit
9. **Implement retry policies** with Polly
10. **Add API versioning**
