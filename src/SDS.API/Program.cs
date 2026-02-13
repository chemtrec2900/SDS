using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using SDS.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "SDS Management API", Version = "v1" });
    // JWT Bearer token configuration will be added when Microsoft.OpenApi package is available
    // For now, authentication can be configured via Swagger UI manually
});

// Database configuration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? "Server=(localdb)\\mssqllocaldb;Database=SDSDb;Trusted_Connection=True;MultipleActiveResultSets=true";

builder.Services.AddDbContext<SdsDbContext>(options =>
    options.UseSqlServer(connectionString));

// Authentication & Authorization
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    // Configure JWT Bearer token validation
    options.Authority = builder.Configuration["Authentication:Authority"];
    options.Audience = builder.Configuration["Authentication:Audience"];
    options.RequireHttpsMetadata = false; // Set to true in production
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };
})
.AddOpenIdConnect(options =>
{
    // Configure OIDC for SSO (Entra ID/Okta/Google)
    options.Authority = builder.Configuration["Authentication:Oidc:Authority"];
    options.ClientId = builder.Configuration["Authentication:Oidc:ClientId"];
    options.ClientSecret = builder.Configuration["Authentication:Oidc:ClientSecret"];
    options.ResponseType = "code";
    options.SaveTokens = true;
    options.GetClaimsFromUserInfoEndpoint = true;
    options.Scope.Add("openid");
    options.Scope.Add("profile");
    options.Scope.Add("email");
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
    options.AddPolicy("Author", policy => policy.RequireRole("Author", "Admin"));
    options.AddPolicy("Reviewer", policy => policy.RequireRole("Reviewer", "Admin"));
    options.AddPolicy("Viewer", policy => policy.RequireRole("Viewer", "Author", "Reviewer", "Admin"));
});

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Application services
builder.Services.AddScoped<SDS.Application.Interfaces.ISdsService, SDS.Infrastructure.Services.SdsService>();
builder.Services.AddScoped<SDS.Application.Interfaces.IAuditService, SDS.Infrastructure.Services.AuditService>();
// builder.Services.AddScoped<ILibraryService, LibraryService>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Ensure database is created (for development only)
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<SdsDbContext>();
    dbContext.Database.EnsureCreated();
}

app.Run();
