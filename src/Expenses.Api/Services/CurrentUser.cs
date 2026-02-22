using System.Security.Claims;
using Expenses.Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Expenses.Api.Services;

public class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetId()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        if (user?.Identity?.IsAuthenticated != true)
            throw new InvalidOperationException("User is not authenticated.");

        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier)
                     ?? user.FindFirstValue("sub");
        if (string.IsNullOrEmpty(userId))
            throw new InvalidOperationException("User ID claim not found.");

        return userId;
    }
}
