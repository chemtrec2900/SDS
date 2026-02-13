# Setup Guide - SDS Management System

## Prerequisites

- .NET 9 SDK ([Download](https://dotnet.microsoft.com/download))
- SQL Server (LocalDB, Express, or full instance)
- Visual Studio 2022 or VS Code (recommended)
- Node.js 18+ (for frontend development, when implemented)

## Initial Setup

### 1. Clone and Navigate
```bash
cd C:\Users\tesfaye.gari\source\repos\SDS
```

### 2. Restore NuGet Packages
```bash
dotnet restore
```

### 3. Configure Database Connection

Edit `src/SDS.API/appsettings.json` and update the connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=SDSDb;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

### 4. Create Database

The database will be automatically created on first run. Alternatively, you can use Entity Framework migrations:

```bash
cd src/SDS.Infrastructure
dotnet ef migrations add InitialCreate --startup-project ../SDS.API
dotnet ef database update --startup-project ../SDS.API
```

### 5. Configure Authentication

Update `appsettings.json` with your OIDC provider settings:

```json
{
  "Authentication": {
    "Authority": "https://login.microsoftonline.com/{your-tenant-id}",
    "Audience": "api://your-api-resource-id",
    "Oidc": {
      "Authority": "https://login.microsoftonline.com/{your-tenant-id}/v2.0",
      "ClientId": "your-client-id",
      "ClientSecret": "your-client-secret"
    }
  }
}
```

For development/testing, you can temporarily disable authentication by commenting out the `[Authorize]` attributes in controllers.

### 6. Run the Application

```bash
cd src/SDS.API
dotnet run
```

The API will be available at:
- HTTP: `http://localhost:5000`
- HTTPS: `https://localhost:5001`
- Swagger UI: `https://localhost:5001/swagger`

## Database Seeding (Optional)

Create a seed data script to populate initial roles and test data:

```csharp
// Example seed data
var roles = new[]
{
    new Role { Id = Guid.NewGuid(), Type = RoleType.Admin, Name = "Admin" },
    new Role { Id = Guid.NewGuid(), Type = RoleType.Author, Name = "Author" },
    new Role { Id = Guid.NewGuid(), Type = RoleType.Reviewer, Name = "Reviewer" },
    // ... other roles
};
```

## Testing the API

### Using Swagger UI
1. Navigate to `https://localhost:5001/swagger`
2. Use the interactive API explorer to test endpoints

### Using curl
```bash
# Get SDS documents (requires authentication)
curl -X GET "https://localhost:5001/api/sds/search" \
  -H "Authorization: Bearer {your-token}"

# Create SDS
curl -X POST "https://localhost:5001/api/sds" \
  -H "Authorization: Bearer {your-token}" \
  -H "Content-Type: application/json" \
  -d '{"documentNumber": "SDS-001", "productName": "Test Chemical"}'
```

### Using Postman
1. Import the OpenAPI spec from Swagger
2. Configure authentication (Bearer token)
3. Test endpoints

## Development Workflow

### Adding New Features

1. **Domain Layer**: Add entities to `SDS.Domain/Entities`
2. **Application Layer**: Add interfaces and DTOs to `SDS.Application`
3. **Infrastructure Layer**: Implement services in `SDS.Infrastructure/Services`
4. **API Layer**: Add controllers to `SDS.API/Controllers`
5. **Register Services**: Update `Program.cs` with dependency injection

### Database Migrations

```bash
# Create migration
dotnet ef migrations add MigrationName --project src/SDS.Infrastructure --startup-project src/SDS.API

# Update database
dotnet ef database update --project src/SDS.Infrastructure --startup-project src/SDS.API

# Remove last migration (if needed)
dotnet ef migrations remove --project src/SDS.Infrastructure --startup-project src/SDS.API
```

## Troubleshooting

### Database Connection Issues
- Verify SQL Server is running
- Check connection string format
- Ensure database server allows connections

### Authentication Errors
- Verify OIDC configuration
- Check token expiration
- Review claims mapping

### Build Errors
- Run `dotnet clean` then `dotnet restore`
- Check .NET SDK version: `dotnet --version` (should be 9.x)
- Verify all project references are correct

## Next Steps

1. **Implement Frontend**: Set up React application
2. **Add Storage**: Configure Azure Blob Storage or AWS S3
3. **Set Up Search**: Integrate Elasticsearch or Azure Cognitive Search
4. **Add Caching**: Configure Redis
5. **Set Up CI/CD**: Configure Azure DevOps or GitHub Actions
6. **Add Monitoring**: Set up Application Insights or similar

## Production Deployment

### Azure App Service
1. Create App Service plan
2. Configure connection strings
3. Set up managed identity
4. Configure authentication
5. Deploy via Azure DevOps or GitHub Actions

### AWS ECS/Fargate
1. Create ECS cluster
2. Configure RDS database
3. Set up S3 for storage
4. Configure ALB and security groups
5. Deploy containerized application

## Support

For issues or questions, refer to:
- Architecture documentation: `docs/ARCHITECTURE.md`
- API documentation: Swagger UI at `/swagger`
- Code comments and XML documentation
