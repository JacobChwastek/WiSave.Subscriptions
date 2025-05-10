namespace WiSave.Subscriptions.Domain.Subscriptions.ValueObjects;

internal record TrialPeriod(DateOnly StartDate, int DurationInDays);