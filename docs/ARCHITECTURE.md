# SDS Management System - Architecture Documentation

## Overview

The Safety Datasheet Management System is built using Clean Architecture principles with a multi-tenant architecture supporting enterprise-grade features for SDS management, version control, document distribution, and compliance.

## Technology Stack

### Backend
- **.NET 9** - Core framework
- **ASP.NET Core Web API** - RESTful API layer
- **Entity Framework Core 9** - ORM and data access
- **SQL Server** - Primary database
- **JWT/OIDC** - Authentication and authorization

### Frontend (To be implemented)
- **React** with TypeScript
- **Material-UI** or similar component library
- **React Query** for data fetching
- **React Router** for navigation

### Infrastructure (Planned)
- **Azure Blob Storage** / **AWS S3** - Document storage
- **Redis** - Caching layer
- **Elasticsearch** / **Azure Cognitive Search** - Full-text search
- **Azure Service Bus** / **RabbitMQ** - Message queue for notifications

## Architecture Layers

### Domain Layer (`SDS.Domain`)
Contains core business entities and domain logic:
- **Entities**: Tenant, User, Role, SdsDocument, SdsSection, Library, etc.
- **Enums**: RoleType, SdsStatus, SectionNumber, AuditAction, etc.
- No dependencies on other layers

### Application Layer (`SDS.Application`)
Contains application logic and use cases:
- **Interfaces**: ISdsService, ILibraryService, IAuditService, IRepository
- **DTOs**: Data transfer objects for API communication
- Depends only on Domain layer

### Infrastructure Layer (`SDS.Infrastructure`)
Contains implementations of external concerns:
- **Data**: DbContext, Entity Framework configurations
- **Repositories**: Generic repository pattern implementation
- **Services**: Service implementations (SdsService, AuditService)
- **External Services**: Email, SMS, Storage, Search integrations
- Depends on Domain and Application layers

### API Layer (`SDS.API`)
Contains API controllers and configuration:
- **Controllers**: RESTful API endpoints
- **Middleware**: Authentication, authorization, error handling
- **Configuration**: Dependency injection, CORS, Swagger
- Depends on Application and Infrastructure layers

## Multi-Tenant Architecture

### Tenant Isolation
- Each tenant has isolated data through `TenantId` foreign keys
- Row-level security enforced at application layer
- Database-level isolation can be added for enhanced security

### Library Management
The system supports three types of libraries:
1. **Tenant Library** - Shared across all users in a tenant
2. **User Library** - Personal library for individual users
3. **Main Repository** - Global repository accessible to all tenants (with restrictions)

### Access Control
- **Admin**: Full access across all libraries
- **ESS (Support Staff)**: Full access across all libraries
- **Management**: Access to repository and own library
- **Downstream Users**: Access only to their library
- **Restricted Documents**: Only Admin and ESS can access

## Key Features Implementation

### SDS Management
- **Creation**: Template-based SDS creation with all 16 sections
- **Versioning**: Full version history with diff tracking
- **Review Workflow**: Submit → Review → Approve/Reject cycle
- **Import**: PDF/Word/JSON import with content mapping

### Document Distribution
- **Email Sharing**: Direct email with SDS attachment
- **Secure Links**: Time-limited, access-controlled sharing links
- **Intranet Embedding**: Iframe embedding with domain restrictions
- **QR Codes**: Dynamic QR code generation for quick access

### GHS Label Generation
- **Pictograms**: Standard GHS pictogram rendering
- **H/P Statements**: Hazard and Precautionary statements
- **Signal Words**: Danger/Warning classification
- **PDF Export**: Print-ready label generation

### Search & Discovery
- **Full-text Search**: Chemical name, CAS number, supplier
- **Faceted Search**: Filter by hazard, supplier, status
- **Synonyms**: Trade name mapping
- **Shared SDS**: Public repository search

### Audit Trail
- **Comprehensive Logging**: All actions logged with metadata
- **User Tracking**: Who did what and when
- **Export**: CSV/PDF export for compliance
- **Retention**: Configurable retention policies

## Security

### Authentication
- **SSO Support**: Entra ID, Okta, Google via OIDC
- **MFA**: Multi-factor authentication support
- **Conditional Access**: IP restrictions, device policies
- **Legacy Auth Blocking**: Configurable per tenant

### Authorization
- **Role-Based Access Control (RBAC)**: Admin, Author, Reviewer, etc.
- **Policy-Based**: Attribute-based authorization policies
- **Resource-Level**: Per-document access control

### Data Protection
- **Encryption at Rest**: Database encryption
- **Encryption in Transit**: HTTPS/TLS
- **API Keys**: Secure API key management with rotation
- **Audit Logging**: Security event tracking

## Performance Considerations

### Caching Strategy
- **Redis**: Frequently accessed SDS metadata
- **CDN**: Static assets and document downloads
- **In-Memory**: User sessions and permissions

### Database Optimization
- **Indexing**: Strategic indexes on search fields
- **Partitioning**: Large tables partitioned by tenant
- **Query Optimization**: Eager loading, projection queries

### Scalability
- **Horizontal Scaling**: Stateless API design
- **Load Balancing**: Multiple API instances
- **Database Sharding**: Tenant-based sharding (future)

## API Design

### RESTful Principles
- Resource-based URLs
- HTTP verbs for actions
- Status codes for responses
- JSON for data exchange

### Versioning
- URL versioning: `/api/v1/sds`
- Header versioning support
- Backward compatibility maintained

### Documentation
- Swagger/OpenAPI documentation
- Interactive API explorer
- Code examples

## Deployment

### Development
- Local SQL Server database
- In-memory storage for documents
- Development authentication bypass

### Production
- Azure App Service / AWS ECS
- Managed SQL Database
- Blob Storage for documents
- Redis Cache
- Application Insights / CloudWatch monitoring

## Future Enhancements

1. **Machine Learning**: Auto-classification of hazards
2. **Regulatory Compliance**: Automated compliance checking
3. **Mobile Apps**: Native iOS/Android applications
4. **Offline Support**: Progressive Web App capabilities
5. **Advanced Analytics**: Predictive risk assessment
6. **Integration Marketplace**: Pre-built connectors
