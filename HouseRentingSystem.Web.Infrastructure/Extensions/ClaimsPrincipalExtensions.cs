namespace HouseRentingSystem.Web.Infrastructure.Extensions;

using System.Security.Claims;

using static HouseRentingSystem.Common.GeneralApplicationConstants;

public static class ClaimsPrincipalExtensions
{
    /// <summary>
    /// Returns currently logged users ID
    /// </summary>
    /// <param name="user">Current user from claims</param>
    /// <returns></returns>
    public static string GetId(this ClaimsPrincipal user)
    {
        return user.FindFirstValue(ClaimTypes.NameIdentifier)!;
    }

    /// <summary>
    /// Check if currently logged user is in the admin role
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public static bool IsUserAdmin(this ClaimsPrincipal user)
    {
        return user.IsInRole(AdminRoleName);
    }
}
