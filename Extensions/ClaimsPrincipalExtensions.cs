using System.Security.Claims;

public static class ClaimsPrincipalExtensions
{
    public static string GetClaimStringValue(this ClaimsPrincipal principal, string key)
    {
        var claim = principal.Claims.FirstOrDefault(x => x.Type.Equals(key));
        return claim != null ? claim.Value : string.Empty;
    }
}