using Infrastructure.Auditing.Extensions;
using Infrastructure.Auditing.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Infrastructure.Persistence.Contexts;

public class AuditDbContext : DbContext
{
    private const string DOMAIN_SCHEMA = "Audit";
    public AuditDbContext(DbContextOptions<AuditDbContext> options) : base(options)
    {
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
=> optionsBuilder.LogTo(message => Debug.WriteLine(message));

    public DbSet<SaveChangesAudit> SaveChangesAudits { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<AuditHistory>().ToTable("AuditHistory", DOMAIN_SCHEMA).Ignore(t => t.AutoHistoryDetails);
        builder.Entity<AuditHistory>(b =>
        {
            b.Property(c => c.Id).UseIdentityColumn(); //TODO: Possibly change this to avoid integer overflow, or cleanup every once in a while
            b.Property(c => c.PrimaryKey).IsRequired().HasMaxLength(128);
            b.Property(c => c.TableName).IsRequired().HasMaxLength(128);
            //b.Property(c => c.Changed).HasMaxLength(2048);
            b.Property(c => c.Username).HasMaxLength(128);
            // This MSSQL only
            b.Property(c => c.Created).HasDefaultValueSql("getdate()");
        });

        builder.Entity<SaveChangesAudit>().ToTable("SaveChangesAudit", DOMAIN_SCHEMA);
    }
}
