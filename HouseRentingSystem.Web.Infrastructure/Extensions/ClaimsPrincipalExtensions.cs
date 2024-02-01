using System.Security.Claims;

namespace HouseRentingSystem.Web.Infrastructure.Extensions;

public static class ClaimsPrincipalExtensions
{
    /// <summary>
    /// Returns currently logged users ID.
    /// </summary>
    /// <param name="user">Current user from claims</param>
    /// <returns></returns>
    public static string GetId(this ClaimsPrincipal user)
    {
        return user.FindFirstValue(ClaimTypes.NameIdentifier)!;
    }
}
