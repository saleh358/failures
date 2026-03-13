using System.Reflection;
using Failures.Application.Interfaces;
using Failures.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Failures.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<FailedPayment> FailedPayments => Set<FailedPayment>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<FailedPayment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.PaymentId).IsRequired();
            entity.Property(e => e.UserId).IsRequired();
            entity.Property(e => e.Amount).HasColumnType("decimal(18,2)");
            entity.Property(e => e.FailureReason).HasMaxLength(255);
            entity.Property(e => e.PaymentProvider).HasMaxLength(100);
            entity.Property(e => e.OccurredAt).IsRequired();
        });
    }
}
