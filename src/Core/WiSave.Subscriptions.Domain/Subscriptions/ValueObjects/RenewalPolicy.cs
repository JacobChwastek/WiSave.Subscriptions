namespace WiSave.Subscriptions.Domain.Subscriptions.ValueObjects;

internal record RenewalPolicy(bool AutoRenew, int MaxRenewals = 0);