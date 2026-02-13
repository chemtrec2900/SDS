# Safety Datasheet Management System (SDS)

A comprehensive enterprise Safety Datasheet Management System with multi-tenant support, version control, document distribution, and regulatory compliance features.

## Architecture

- **Backend**: ASP.NET Core Web API (.NET 8)
- **Frontend**: React with TypeScript
- **Database**: SQL Server / PostgreSQL
- **Authentication**: OIDC (Entra ID/Okta/Google), MFA support
- **Storage**: Azure Blob Storage / AWS S3 for documents
- **Caching**: Redis for performance
- **Search**: Elasticsearch / Azure Cognitive Search

## Features

### Access Management
- SSO integration with Entra ID/Okta/Google via OIDC
- Role-based access control (Admin, Author, Reviewer, Compliance, Inventory Manager, Viewer)
- MFA/Conditional Access enforcement
- Multi-tenant provisioning

### SDS Management
- Create/update SDS from templates (Sections 1-16)
- Import existing SDS (PDF/Word/JSON)
- Version control with diff comparison
- Review workflow (approve/reject)

### Document Distribution
- Email sharing
- Secure expiring links
- Intranet embedding
- Link scoping (public vs authenticated)

### Hazard Communication
- GHS label generation with pictograms
- H/P statements
- QR code generation
- Label printing (PDF)

### Integration & APIs
- RESTful APIs
- Webhook support
- ERP/EHS/CMMS/SharePoint connectors
- API key management

### Additional Features
- Custom reporting and dashboards
- Risk assessment tools
- Chemical inventory management
- Audit trail
- Advanced search with facets
- Notifications & alerts

## Getting Started

### Prerequisites
- .NET 8 SDK
- Node.js 18+
- SQL Server / PostgreSQL
- Redis (optional, for caching)

### Backend Setup
```bash
cd src/SDS.API
dotnet restore
dotnet ef database update
dotnet run
```

### Frontend Setup
```bash
cd src/SDS.Web
npm install
npm start
```

## Project Structure

```
SDS/
├── src/
│   ├── SDS.API/              # Backend API
│   ├── SDS.Application/      # Application layer (services, DTOs)
│   ├── SDS.Domain/           # Domain models
│   ├── SDS.Infrastructure/   # Data access, external services
│   └── SDS.Web/              # React frontend
├── tests/                    # Unit and integration tests
└── docs/                     # Documentation
```

## License

Proprietary - All rights reserved
