using Failures.Domain.Abstraction;
using Failures.Domain.Entities;
using Failures.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Failures.Infrastructure.Repository;

public class FailureRepository : IFailureRepository
{
    private readonly ApplicationDbContext _context;

    public FailureRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddFailureAsync(FailedPayment failedPayment)
    {
        await _context.FailedPayments.AddAsync(failedPayment);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteFailureAsync(Guid id)
    {
        var failure = await _context.FailedPayments.FindAsync(id);
        if (failure != null)
        {
            await _context.FailedPayments.ExecuteDeleteAsync();
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<FailedPayment>> GetAllEntities(int count)
    {
        return await _context.FailedPayments.Take(count).ToListAsync();
    }

    public async Task<FailedPayment?> GetFailureByIdAsync(Guid id)
    {
        return await _context.FailedPayments.FindAsync(id);
    }
}
