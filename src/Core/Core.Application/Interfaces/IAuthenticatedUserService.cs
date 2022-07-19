using Core.Domain.Enumerations;

namespace Core.Application.Interfaces;

public interface IAuthenticatedUserService
{
    string UserId { get; }
    public string Username { get; }
    public string Name { get; }
    public IEnumerable<RolesEnum> Roles { get; }
}
