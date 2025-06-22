using MassTransit;
using WiSave.Subscriptions.Contracts.Dtos;
using WiSave.Subscriptions.Contracts.Queries;
using WiSave.Subscriptions.Projections.Repository;

namespace WiSave.Subscriptions.Projections.QueryHandlers;

internal class GetSubscriptionsQueryHandler(ISubscriptionRepository repository) : IConsumer<GetSubscriptionsQuery>
{
    public async Task Consume(ConsumeContext<GetSubscriptionsQuery> context)
    {
        var query = context.Message;

        var pageNumber = query.PageNumber <= 0 ? 1 : query.PageNumber;
        var pageSize = query.PageSize <= 0 ? 10 : query.PageSize;

        var skip = (pageNumber - 1) * pageSize;

        var (subscriptions, totalCount) = await repository.GetByUserIdAsync(
            query.UserId,
            query.Name,
            query.Plan,
            query.IsTrial,
            skip,
            pageSize,
            context.CancellationToken);

        var dtos = subscriptions.Select(s => new SubscriptionDto(
            s.Id,
            s.Logo,
            s.Name,
            s.Plan,
            s.Money,
            s.PeriodUnit,
            s.PeriodInterval,
            s.AutoRenew,
            s.StartDate,
            s.IsTrial,
            s.MaxRenewals,
            s.TrialDurationInDays
        )).ToList();

        var paged = PagedResult<SubscriptionDto>.Create(dtos, totalCount, pageNumber, pageSize);

        await context.RespondAsync(new GetSubscriptionsQueryResult(paged));
    }
}