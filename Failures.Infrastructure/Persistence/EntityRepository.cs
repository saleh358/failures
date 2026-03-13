using Failures.Domain.Abstraction;
using Failures.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Failures.Infrastructure.Persistence;

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
            _context.FailedPayments.Remove(failure);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<FailedPayment>> GetAllEntities()
    {
        return await _context.FailedPayments.ToListAsync();
    }

    public async Task<FailedPayment> GetFailureByIdAsync(Guid id)
    {
        return await _context.FailedPayments.FindAsync(id);
    }

    public async Task UpdateFailureAsync(FailedPayment failedPayment)
    {
        _context.FailedPayments.Update(failedPayment);
        await _context.SaveChangesAsync();
    }
}
