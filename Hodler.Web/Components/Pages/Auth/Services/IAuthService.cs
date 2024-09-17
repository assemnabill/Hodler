using Hodler.Contracts.User;

namespace Hodler.Web.Components.Pages.Auth.Services;

public interface IAuthService
{
    Task<RegisterResult> Register(RegisterModel registerModel);
    Task<LoginResult> Login(LoginModel loginModel);
    Task Logout();
}