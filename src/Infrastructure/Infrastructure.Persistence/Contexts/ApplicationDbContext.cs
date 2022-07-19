using Core.Domain.Entities;
using Core.Domain.Entities.Application;
using Infrastructure.Auditing.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Transactions;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Core.Application.Interfaces;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Infrastructure.Persistence.Contexts;

public class ApplicationDbContext : DbContext
{
    private readonly IAuthenticatedUserService _authenticatedUser;
    private const string DOMAIN_SCHEMA = "Domain";
    private readonly ISaveChangesInterceptor _auditInterceptor;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ISaveChangesInterceptor auditInterceptor,
        IAuthenticatedUserService authenticatedUser) : base(options)
    {
        _auditInterceptor = auditInterceptor;
        _authenticatedUser = authenticatedUser;
    }
    public virtual DbSet<Address> Addresses { get; set; } = null!;
    public virtual DbSet<Contact> Contacts { get; set; } = null!;
    public virtual DbSet<State> States { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
         => optionsBuilder
            .AddInterceptors(_auditInterceptor)
            .LogTo(message => Debug.WriteLine(message));


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
       // builder.EnableAuditHistory();

        //Contact
        builder.Entity<Contact>().ToTable("Contact", DOMAIN_SCHEMA);
        builder.Entity<Contact>(c =>
        {
            c.Property(e => e.Id).HasMaxLength(256);
            c.Property(e => e.FirstName).IsRequired().HasMaxLength(512);
            c.Property(e => e.Company).HasMaxLength(1024);
            c.HasMany(c => c.Addresses).WithOne(a => a.Contact).HasForeignKey(c => c.ContactId).OnDelete(DeleteBehavior.Cascade);
        });

        //Address
        builder.Entity<Address>().ToTable("Address", DOMAIN_SCHEMA);
        builder.Entity<Address>(a =>
        {
            a.Property(a => a.StreetAddress).IsRequired();

        });

        //State
        builder.Entity<State>().ToTable("State", DOMAIN_SCHEMA);
        builder.Entity<State>(s =>
        {
            s.Property(s => s.StateName).IsRequired();
            s.HasMany(s => s.Addresses).WithOne(a => a.State).HasForeignKey(s => s.StateId).OnDelete(DeleteBehavior.Cascade);
        });
    }

    /// <summary>
    /// Overrides the <see cref="SaveChanges(bool)"/>
    /// </summary>
    /// <param name="acceptAllChangesOnSuccess"></param>
    /// <returns></returns>
    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        DbContextUpdateOperations.UpdateDates(ChangeTracker.Entries<AuditableEntity>(), _authenticatedUser.Username);
        //this.EnsureAuditHistory("Username", _auditContext);
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    /// <summary>
    /// Overrides the <see cref="DbContext.SaveChanges()"/>.
    /// </summary>
    /// <returns></returns>
    public override int SaveChanges()
    {
        DbContextUpdateOperations.UpdateDates(ChangeTracker.Entries<AuditableEntity>(), "Username");
        //this.EnsureAuditHistory("Username", _auditContext);
        return base.SaveChanges(true);
    }

    /// <summary>
    /// Overrides the <see cref="DbContext.SaveChangesAsync(bool, CancellationToken)"/>
    /// </summary>
    /// <param name="acceptAllChangesOnSuccess"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        DbContextUpdateOperations.UpdateDates(ChangeTracker.Entries<AuditableEntity>(), "Username");
        //this.EnsureAuditHistory("Username", _auditContext);
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    /// <summary>
    /// Overrides the <see cref="SaveChangesAsync(CancellationToken)"/>
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        
        //Update dates to the entity
        DbContextUpdateOperations.UpdateDates(ChangeTracker.Entries<AuditableEntity>(), _authenticatedUser.Username);

        var result = await base.SaveChangesAsync(true, cancellationToken).ConfigureAwait(false);

        return result;
    }
}
