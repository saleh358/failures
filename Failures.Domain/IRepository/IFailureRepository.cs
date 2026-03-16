using Failures.Domain.Entities;

namespace Failures.Domain.Abstraction;

public interface IFailureRepository
{
    public Task<FailedPayment> GetFailureByIdAsync(Guid id);
    public Task<IEnumerable<FailedPayment>> GetAllEntities(int count);
    public Task AddFailureAsync(FailedPayment failedPayment);
    public Task DeleteFailureAsync(Guid id);
}
