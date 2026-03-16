using Failures.Domain.Entities;

namespace Failures.Domain.IService;

public interface IFailureService
{
    public Task<IEnumerable<FailedPayment>> GetAllFailuresAsync(int count);
}
