using MassTransit;
using WiSave.Subscriptions.Contracts.Dtos;
using WiSave.Subscriptions.Contracts.Queries;
using WiSave.Subscriptions.Projections.Repository;

namespace WiSave.Subscriptions.Projections.QueryHandlers;

public class GetSubscriptionQueryHandler(ISubscriptionRepository repository) : IConsumer<GetSubscriptionQuery>
{
    public async Task Consume(ConsumeContext<GetSubscriptionQuery> context)
    {
        var message = context.Message;

        var subscription = await repository.GetByIdAsync(message.Id, context.CancellationToken);

        var dto = new SubscriptionDto(subscription.Id, 
            subscription.Name, 
            subscription.Plan, 
            subscription.Money, 
            subscription.PeriodUnit,
            subscription.PeriodInterval, 
            subscription.AutoRenew,
            subscription.StartDate, 
            subscription.IsTrial,
            subscription.MaxRenewals, 
            subscription.TrialDurationInDays);

        await context.RespondAsync(new GetSubscriptionQueryResult(dto));
    }
}