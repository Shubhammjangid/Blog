using System.Security.Claims;
using Entities;
using Services;

namespace BlogManagement.Helper;

public class CookieUserDetailsHandler
{
    private readonly UserService userService;
    public CookieUserDetailsHandler(UserService userService)
    {
        this.userService = userService;
    }

    public async Task<User?> GetUserDetail(ClaimsIdentity? claimsIdentity)
    {
        if(!claimsIdentity.Claims.Any())
        {
            return null;
        }

        var requestorEmailId = claimsIdentity.Claims.FirstOrDefault(e => e.Type.Contains("emailaddress")).Value;
        
        try
        {
            return await userService.GetUserProfile(requestorEmailId);
        }
        catch
        {
            return null;
        }
    }
}