using System.Threading;
using System.Threading.Tasks;
using Failures.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Failures.Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<FailedPayment> FailedPayments { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
