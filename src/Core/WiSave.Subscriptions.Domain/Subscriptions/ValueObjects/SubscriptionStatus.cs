namespace WiSave.Subscriptions.Domain.Subscriptions.ValueObjects;

public enum SubscriptionStatus
{
    Pending,        // Created but not yet active
    Active,         // Billing and usage allowed
    Paused,         // Temporarily inactive
    Cancelled,      // Manually stopped
    Expired,        // Ended due to time/term
    Trial,          // In free trial phase
    Suspended       // Disabled due to failed payment or violation
}