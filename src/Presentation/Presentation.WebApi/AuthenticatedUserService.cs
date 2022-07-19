using Core.Application.Interfaces;
using Core.Domain.Enumerations;
using System.Security.Claims;

namespace Presentation.WebApi;

public class AuthenticatedUserService : IAuthenticatedUserService
{
    public AuthenticatedUserService(IHttpContextAccessor httpContextAccessor)
    {
        UserId = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;
        Username = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value ?? "Default User";
        //Name = httpContextAccessor.HttpContext?.User?.FindFirst(ApplicationUser.FullNameClaimType)?.Value ?? null;
        //Culture = httpContextAccessor.HttpContext?.User?.FindFirst(ApplicationUser.CultureClaimType)?.Value ?? null;
        //UiCulture = httpContextAccessor.HttpContext?.User?.FindFirst(ApplicationUser.UiCultureClaimType)?.Value ?? null;
        //var profilePictureClaim = httpContextAccessor.HttpContext?.User?.FindFirst(ApplicationUser.ProfilePictureClaimType)?.Value;
        //if (profilePictureClaim != null)
        //{
        //    ProfilePicture = profilePictureClaim;
        //}

        //var roles = httpContextAccessor.HttpContext?.User?.Claims.Where(c => c.Type == ClaimTypes.Role);
        //if (roles != null)
        //    Roles = roles.Select(r => r.Value.ToEnum<RolesEnum>());
    }

    public string UserId { get; }

    public string Username { get; }
    public string Name { get; }
    public string ProfilePicture { get; }
    public string Culture { get; set; }
    public string UiCulture { get; set; }
    public IEnumerable<RolesEnum> Roles { get; }
}
