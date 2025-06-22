namespace WiSave.Subscriptions.WebApi.Models;

public record UserContext
{
    public string? UserId { get; init; }
    public string? Email { get; init; }
    public string[] Roles { get; init; } = [];
    public bool IsAuthenticated { get; init; }
    
    public static UserContext Anonymous => new() { IsAuthenticated = false };
}