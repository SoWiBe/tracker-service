using ErrorOr;

namespace Tracking.Application.Core.Handlers;

public interface ICommandHandler<in TCommand, TResponse>
{
    Task<ErrorOr<TResponse>> HandleAsync(TCommand command, CancellationToken ct = default);
}

public interface ICommandHandler<in TCommand>
{
    Task<ErrorOr<Success>> HandleAsync(TCommand command, CancellationToken ct = default);
}

public interface IQueryHandler<in TQuery, TResponse>
{
    Task<ErrorOr<TResponse>> HandleAsync(TQuery query, CancellationToken ct = default);
}