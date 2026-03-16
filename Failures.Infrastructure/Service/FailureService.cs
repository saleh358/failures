using Failures.Domain.Abstraction;
using Failures.Domain.Entities;
using Failures.Domain.IService;

namespace Failures.Infrastructure.Service;

public class FailureService(IFailureRepository _failureRepository) : IFailureService
{
    public Task<IEnumerable<FailedPayment>> GetAllFailuresAsync(int count)
    {
        return _failureRepository.GetAllEntities(count);
    }
}
