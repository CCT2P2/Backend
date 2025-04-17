using System.Security.Claims;

namespace gnufv2.Extensions;

public static class ClaimsPrincipalExtensions
{

    /// <summary>
    /// Extends the ClaimsPrincipal for users to include a method for checking if the user owns an item
    /// Im just gonna explain how all this works here, cus it seems like the best place: <br/><br/>
    /// When a controller includes the [Authorize] attribute, it expects the request to include an Authentication header with the content "Bearer [token]"<br/>
    /// This token includes all sort of claims about the user (like id, username etc), as well as a signature which is automatically validated by the backend.<br/>
    /// Beyond that we can also use the claims to verify other things, such as in this method where we verity that the "NameIdentifier" claim (aka the user id) matches the owner of an item.<br/>
    /// This is for instance used for editing a post. If we didnt check, anyone logged in could simply just send a request directly to the api to edit a post, even if they arent the author.<br/>
    /// </summary>
    /// <param name="user">The user to check for</param>
    /// <param name="itemOwnerId">The id of the user that owns the item</param>
    /// <returns>A boolean indication whether the user owns the item or not</returns>
    public static bool MatchesId(this ClaimsPrincipal user, string itemOwnerId)
    {
        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return !string.IsNullOrEmpty(userId) && itemOwnerId == userId;
    }
}