using Failures.Domain.Entities;
using Failures.Domain.IService;
using MediatR;

namespace Failures.Application.Features.Failures.Queries;

public record GetFailures : IRequest<List<FailedPayment>>
{
    public int Count { get; init; } = 10;
}

public record FailedPaymentDto(Guid Id, string? ErrorMessage, DateTime OccurredAt);

public class GetRecentFailuresQueryHandler(IFailureService _failureService)
    : IRequestHandler<GetFailures, IEnumerable<FailedPayment>>
{
    public async Task<IEnumerable<FailedPayment>> Handle(
        GetFailures request,
        CancellationToken cancellationToken
    )
    {
        IEnumerable<FailedPayment> failures = await _failureService.GetAllFailuresAsync(
            request.Count
        );
        return failures;
    }
}
