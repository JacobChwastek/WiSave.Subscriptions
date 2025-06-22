using WiSave.Subscriptions.WebApi.Models;

namespace WiSave.Subscriptions.WebApi.Services;

public interface IUserContextService
{
    UserContext GetUserContext();
    string? GetUserId();
    string? GetUserEmail();
    string[] GetUserRoles();
    bool IsInRole(string role);
    bool IsAuthenticated { get; }
}