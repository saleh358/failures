using System;

namespace Failures.Domain.Entities;

public record FailedPayment
{
    public Guid Id { get; init; }
    public Guid PaymentId { get; init; }
    public Guid UserId { get; init; }
    public decimal Amount { get; init; }
    public string FailureReason { get; init; } = string.Empty;
    public string PaymentProvider { get; init; } = string.Empty;
    public DateTime OccurredAt { get; init; }
}
