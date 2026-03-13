using Failures.Application.Interfaces;
using Failures.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Failures.Application.Features.Failures.Queries;

public record GetRecentFailuresQuery : IRequest<List<FailedPayment>>
{
    public int Count { get; init; } = 10;
}

public record FailedPaymentDto(Guid Id, string? ErrorMessage, DateTime OccurredAt);

public class GetRecentFailuresQueryHandler(IApplicationDbContext _context)
    : IRequestHandler<GetRecentFailuresQuery, List<FailedPayment>>
{
    public async Task<List<FailedPayment>> Handle(
        GetRecentFailuresQuery request,
        CancellationToken cancellationToken
    )
    {
        return await _context
            .FailedPayments.OrderByDescending(f => f.OccurredAt)
            .Take(request.Count)
            .ToListAsync(cancellationToken);
    }
}
