using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Serializers;
using WiSave.Shared.Types;
using WiSave.Subscriptions.Projections.Database.Serializers;

namespace WiSave.Subscriptions.Projections.Models;

public class Subscription
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }

    [BsonElement("name")]
    public string Name { get; set; } = null!;

    [BsonElement("logo")]
    public string Logo { get; set; } = string.Empty;

    [BsonElement("userId")]
    [BsonRepresentation(BsonType.String)]
    public Guid UserId { get; set; }

    [BsonElement("periodUnit")]
    [BsonRepresentation(BsonType.String)]
    public PeriodUnit PeriodUnit { get; set; }

    [BsonElement("periodInterval")]
    public int PeriodInterval { get; set; }

    [BsonElement("autoRenew")]
    public bool AutoRenew { get; set; }

    [BsonElement("startDate")]
    [BsonSerializer(typeof(DateOnlySerializer))]
    public DateOnly StartDate { get; set; }

    [BsonElement("endDate")]
    [BsonSerializer(typeof(NullableDateOnlySerializer))]
    public DateOnly? EndDate { get; set; }

    [BsonElement("status")]
    [BsonRepresentation(BsonType.String)]
    public string Status { get; set; } = "Active";

    [BsonElement("isTrial")]
    public bool IsTrial { get; set; }

    [BsonElement("maxRenewals")]
    public int? MaxRenewals { get; set; }

    [BsonElement("trialDurationInDays")]
    public int? TrialDurationInDays { get; set; }

    [BsonElement("plans")]
    public List<SubscriptionPlan> Plans { get; set; } = new();
    
    [BsonIgnore]
    public SubscriptionPlan? ActivePlan => Plans.FirstOrDefault(p => p.IsActive);

    [BsonIgnore]
    public Money? CurrentPrice => ActivePlan?.CurrentPrice;

    [BsonIgnore]
    public string? CurrentPlanName => ActivePlan?.Name;
    
    [BsonIgnore]
    public Money? TotalSpent => CalculateTotalSpent();

    [BsonIgnore]
    public Money? MonthlyValue => CalculateMonthlyValue();

    [BsonIgnore]
    public Money? NextPaymentAmount => CurrentPrice;
    
    [BsonIgnore]
    public int DaysActive => CalculateDaysActive();

    [BsonIgnore]
    public DateOnly? NextPaymentDate => CalculateNextPaymentDate();
    
    [BsonIgnore]
    public bool IsInTrial => IsTrial && CalculateTrialDaysRemaining() > 0;

    [BsonIgnore]
    public int? TrialDaysRemaining => CalculateTrialDaysRemaining();
    
    private Money? CalculateTotalSpent()
    {
        var activePlan = ActivePlan;
        if (activePlan?.CurrentPrice == null) 
            return null;
        
        var completedBillingCycles = CalculateCompletedBillingCycles();
        return activePlan.CurrentPrice * completedBillingCycles;
    }

    private int CalculateCompletedBillingCycles()
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        
        if (StartDate > today)
            return 0;
            
        if (IsInTrial)
            return 0;
            
        var billingStartDate = StartDate;
        
        if (IsTrial && TrialDurationInDays.HasValue)
        {
            billingStartDate = StartDate.AddDays(TrialDurationInDays.Value);
            
            if (billingStartDate > today)
                return 0;
        }
        
        int completedCycles = 0;
        var currentBillingDate = billingStartDate;
        
        while (true)
        {
            var nextBillingDate = PeriodUnit switch
            {
                PeriodUnit.Day => currentBillingDate.AddDays(PeriodInterval),
                PeriodUnit.Week => currentBillingDate.AddDays(PeriodInterval * 7),
                PeriodUnit.Month => currentBillingDate.AddMonths(PeriodInterval),
                PeriodUnit.Year => currentBillingDate.AddYears(PeriodInterval),
                _ => currentBillingDate.AddMonths(1)
            };
            
            if (nextBillingDate > today)
                break;
                
            completedCycles++;
            currentBillingDate = nextBillingDate;
        }
        
        return completedCycles;
    }

    private Money? CalculateMonthlyValue()
    {
        var currentPrice = CurrentPrice;
        if (currentPrice == null) return null;

        return PeriodUnit switch
        {
            PeriodUnit.Month => currentPrice * PeriodInterval,
            PeriodUnit.Week => currentPrice * (decimal)(PeriodInterval * 4.33),
            PeriodUnit.Year => currentPrice * (decimal)PeriodInterval / 12m,
            PeriodUnit.Day => currentPrice * (decimal)(PeriodInterval * 30.44),
            _ => currentPrice
        };
    }

    private int CalculateDaysActive()
    {
        var endDate = EndDate ?? DateOnly.FromDateTime(DateTime.UtcNow);
        return Math.Max(0, (endDate.ToDateTime(TimeOnly.MinValue) - StartDate.ToDateTime(TimeOnly.MinValue)).Days);
    }

    private DateOnly? CalculateNextPaymentDate()
    {
        if (Status != "Active" || !AutoRenew) 
            return null;

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var nextDate = StartDate;
        
        while (nextDate <= today)
        {
            nextDate = PeriodUnit switch
            {
                PeriodUnit.Day => nextDate.AddDays(PeriodInterval),
                PeriodUnit.Week => nextDate.AddDays(PeriodInterval * 7),
                PeriodUnit.Month => nextDate.AddMonths(PeriodInterval),
                PeriodUnit.Year => nextDate.AddYears(PeriodInterval),
                _ => nextDate.AddMonths(1)
            };
        }

        return nextDate;
    }

    private int? CalculateTrialDaysRemaining()
    {
        if (!IsTrial || !TrialDurationInDays.HasValue) 
            return null;
        
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var trialEndDate = StartDate.AddDays(TrialDurationInDays.Value);
        var daysRemaining = (trialEndDate.ToDateTime(TimeOnly.MinValue) - today.ToDateTime(TimeOnly.MinValue)).Days;
        
        return Math.Max(0, daysRemaining);
    }
}