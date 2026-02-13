using Microsoft.EntityFrameworkCore;
using SDS.Domain.Entities;

namespace SDS.Infrastructure.Data;

public class SdsDbContext : DbContext
{
    public SdsDbContext(DbContextOptions<SdsDbContext> options) : base(options)
    {
    }

    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<SdsDocument> SdsDocuments { get; set; }
    public DbSet<SdsSection> SdsSections { get; set; }
    public DbSet<SdsReview> SdsReviews { get; set; }
    public DbSet<Library> Libraries { get; set; }
    public DbSet<LibrarySds> LibrarySds { get; set; }
    public DbSet<SdsShare> SdsShares { get; set; }
    public DbSet<QrCode> QrCodes { get; set; }
    public DbSet<Ghslabel> GhsLabels { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<ChemicalInventory> ChemicalInventories { get; set; }
    public DbSet<RiskAssessment> RiskAssessments { get; set; }
    public DbSet<ApiKey> ApiKeys { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Tenant configuration
        modelBuilder.Entity<Tenant>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Domain).IsUnique();
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
        });

        // User configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.TenantId, e.Email }).IsUnique();
            entity.Property(e => e.Email).IsRequired().HasMaxLength(256);
            entity.HasOne(e => e.Tenant)
                .WithMany(t => t.Users)
                .HasForeignKey(e => e.TenantId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Role configuration
        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Type).IsUnique();
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
        });

        // UserRole composite key
        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.RoleId });
            entity.HasOne(e => e.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(e => e.RoleId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // SDS Document configuration
        modelBuilder.Entity<SdsDocument>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.TenantId, e.DocumentNumber, e.Version }).IsUnique();
            entity.HasIndex(e => e.CasNumber);
            entity.HasIndex(e => e.Status);
            entity.Property(e => e.DocumentNumber).IsRequired().HasMaxLength(100);
            entity.HasOne(e => e.Tenant)
                .WithMany(t => t.SdsDocuments)
                .HasForeignKey(e => e.TenantId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // SDS Section configuration
        modelBuilder.Entity<SdsSection>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.SdsDocumentId, e.SectionNumber });
            entity.HasOne(e => e.SdsDocument)
                .WithMany(d => d.Sections)
                .HasForeignKey(e => e.SdsDocumentId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // SDS Review configuration
        modelBuilder.Entity<SdsReview>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.SdsDocument)
                .WithMany(d => d.Reviews)
                .HasForeignKey(e => e.SdsDocumentId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.Reviewer)
                .WithMany()
                .HasForeignKey(e => e.ReviewerId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Library configuration
        modelBuilder.Entity<Library>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.HasOne(e => e.Tenant)
                .WithMany(t => t.Libraries)
                .HasForeignKey(e => e.TenantId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.Owner)
                .WithMany(u => u.Libraries)
                .HasForeignKey(e => e.OwnerId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // LibrarySds composite key
        modelBuilder.Entity<LibrarySds>(entity =>
        {
            entity.HasKey(e => new { e.LibraryId, e.SdsDocumentId });
            entity.HasOne(e => e.Library)
                .WithMany(l => l.LibrarySds)
                .HasForeignKey(e => e.LibraryId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.SdsDocument)
                .WithMany(d => d.LibrarySds)
                .HasForeignKey(e => e.SdsDocumentId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // SDS Share configuration
        modelBuilder.Entity<SdsShare>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.ShareToken).IsUnique();
            entity.HasOne(e => e.SdsDocument)
                .WithMany(d => d.Shares)
                .HasForeignKey(e => e.SdsDocumentId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // QR Code configuration
        modelBuilder.Entity<QrCode>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Code).IsUnique();
            entity.HasOne(e => e.SdsDocument)
                .WithMany(d => d.QrCodes)
                .HasForeignKey(e => e.SdsDocumentId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // GHS Label configuration
        modelBuilder.Entity<Ghslabel>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.SdsDocument)
                .WithMany(d => d.GhsLabels)
                .HasForeignKey(e => e.SdsDocumentId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Audit Log configuration
        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.TenantId, e.Timestamp });
            entity.HasIndex(e => new { e.EntityType, e.EntityId });
            entity.HasOne(e => e.Tenant)
                .WithMany()
                .HasForeignKey(e => e.TenantId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.User)
                .WithMany(u => u.AuditLogs)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // Notification configuration
        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.TenantId, e.UserId, e.IsRead });
            entity.HasOne(e => e.Tenant)
                .WithMany()
                .HasForeignKey(e => e.TenantId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Chemical Inventory configuration
        modelBuilder.Entity<ChemicalInventory>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.TenantId, e.CasNumber });
            entity.HasOne(e => e.Tenant)
                .WithMany()
                .HasForeignKey(e => e.TenantId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.SdsDocument)
                .WithMany()
                .HasForeignKey(e => e.SdsDocumentId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // Risk Assessment configuration
        modelBuilder.Entity<RiskAssessment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Tenant)
                .WithMany()
                .HasForeignKey(e => e.TenantId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.SdsDocument)
                .WithMany()
                .HasForeignKey(e => e.SdsDocumentId)
                .OnDelete(DeleteBehavior.SetNull);
            entity.HasOne(e => e.ChemicalInventory)
                .WithMany()
                .HasForeignKey(e => e.ChemicalInventoryId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // API Key configuration
        modelBuilder.Entity<ApiKey>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.KeyHash).IsUnique();
            entity.HasOne(e => e.Tenant)
                .WithMany()
                .HasForeignKey(e => e.TenantId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
