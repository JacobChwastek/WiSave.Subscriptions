using System.Security.Claims;
using WiSave.Subscriptions.WebApi.Models;

namespace WiSave.Subscriptions.WebApi.Services;


public class UserContextService(IHttpContextAccessor httpContextAccessor) : IUserContextService
{
    private UserContext? _userContext;

    public UserContext GetUserContext()
    {
        if (_userContext != null)
            return _userContext;

        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext == null)
            return UserContext.Anonymous;
        
        var userId = httpContext.Request.Headers["X-User-Id"].FirstOrDefault();
        var userEmail = httpContext.Request.Headers["X-User-Email"].FirstOrDefault();
        var userRoles = httpContext.Request.Headers["X-User-Roles"]
            .FirstOrDefault()?.Split(',', StringSplitOptions.RemoveEmptyEntries) ?? [];
        
        if (string.IsNullOrEmpty(userId) && httpContext.User.Identity?.IsAuthenticated == true)
        {
            userId = GetUserIdFromClaims(httpContext.User);
            userEmail ??= httpContext.User.FindFirst(ClaimTypes.Email)?.Value;
            
            if (userRoles.Length == 0)
            {
                userRoles = httpContext.User.FindAll(ClaimTypes.Role)
                    .Select(c => c.Value)
                    .ToArray();
            }
        }

        _userContext = new UserContext
        {
            UserId = userId,
            Email = userEmail,
            Roles = userRoles,
            IsAuthenticated = !string.IsNullOrEmpty(userId)
        };

        return _userContext;
    }

    public string? GetUserId()
    {
        return GetUserContext().UserId;
    }

    public string? GetUserEmail() => GetUserContext().Email;
    public string[] GetUserRoles() => GetUserContext().Roles;
    public bool IsInRole(string role) => GetUserContext().Roles.Contains(role, StringComparer.OrdinalIgnoreCase);
    public bool IsAuthenticated => GetUserContext().IsAuthenticated;

    private static string? GetUserIdFromClaims(ClaimsPrincipal user) =>
        user.FindFirst("sub")?.Value ??
        user.FindFirst("userId")?.Value ??
        user.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
        user.FindFirst("id")?.Value;
}