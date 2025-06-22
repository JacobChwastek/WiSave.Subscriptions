using WiSave.Subscriptions.WebApi.Models;
using WiSave.Subscriptions.WebApi.Services;

namespace WiSave.Subscriptions.WebApi.Extensions;

public static class HttpContextExtensions
{
    public static UserContext GetUserContext(this HttpContext httpContext)
    {
        var userContextService = httpContext.RequestServices.GetRequiredService<IUserContextService>();
        return userContextService.GetUserContext();
    }

    public static string? GetUserId(this HttpContext httpContext)
    {
        var userContextService = httpContext.RequestServices.GetRequiredService<IUserContextService>();
        return userContextService.GetUserId();
    }

    public static string RequireUserId(this HttpContext httpContext)
    {
        var userId = httpContext.GetUserId();
        if (string.IsNullOrEmpty(userId))
            throw new UnauthorizedAccessException("User ID is required but not found in the context");
        return userId;
    }

    public static bool IsUserInRole(this HttpContext httpContext, string role)
    {
        var userContextService = httpContext.RequestServices.GetRequiredService<IUserContextService>();
        return userContextService.IsInRole(role);
    }
}