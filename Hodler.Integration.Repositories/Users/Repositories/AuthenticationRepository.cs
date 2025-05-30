using Hodler.Domain.Users.AuthenticationResult;
using Hodler.Domain.Users.Models;
using Hodler.Domain.Users.Ports;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using User = Hodler.Integration.Repositories.Users.Entities.User;

namespace Hodler.Integration.Repositories.Users.Repositories;

public class AuthenticationRepository : IAuthenticationRepository
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public AuthenticationRepository(UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<IUser?> FindByEmailAsync(string email, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
            return null;
        var domainUser = new Domain.Users.Models.User(new UserId(Guid.Parse(user.Id)), null, null);
        domainUser.AddContactInfo(user.UserName, user.PhoneNumber, user.Email); ;
        return domainUser;
    }

    public async Task<IUser?> FindByUserNameAsync(string userName, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(userName);
        if (user is null)
            return null;
        var domainUser = new Domain.Users.Models.User(new UserId(Guid.Parse(user.Id)), null, null);
        domainUser.AddContactInfo(user.UserName, user.PhoneNumber, user.Email);
        return domainUser;
    }

    public async Task<bool> IsExistUserAsync(string? userName, string email)
    {
        return await _userManager.Users.AnyAsync(u => u.UserName == userName
                               || u.Email == email);
    }

    public async Task<LoginResult> LoginAsync(IUser user, string password)
    {
        var entity = await _userManager.Users.SingleOrDefaultAsync(u => u.Id == user.Id.Value.ToString());
        var identityResult = await _signInManager.PasswordSignInAsync(entity, password, false, false);
        var loginResult = new LoginResult
        {
            Succeeded = identityResult.Succeeded
        };
        return loginResult;
    }

    public async Task<RegisterResult> RegisterAsync(IUser user, string password)
    {
        var registerResult = new RegisterResult { Succeeded = true };
        var entity = user.Adapt<User>();
        entity.Email = user.ContactInfo.Email;
        entity.PhoneNumber = user.ContactInfo.PhoneNumber;
        entity.UserName = user.ContactInfo.UserName;

        var identityResult = await _userManager.CreateAsync(entity, password);
        if (identityResult.Succeeded)
            return registerResult;
        registerResult.Succeeded = false;
        registerResult.Errors = identityResult.Errors
                               .Select(e => e.Description)
                               .ToList();
        return registerResult;
    }

    public async Task<bool> ResetPasswordAsync(string email, string newPassword, string token)
    {
        var user = await _userManager.FindByEmailAsync(email);
        var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
        return result.Succeeded;
    }

    public async Task<string> GenerateResetPasswordTokenAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        return token;
    }

    public async Task<string> GenerateConfirmEmailTokenAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        return token;
    }

    public async Task<bool> ConfirmEmailAsync(string email, string token)
    {
        var user = await _userManager.FindByEmailAsync(email);
        var result = await _userManager.ConfirmEmailAsync(user, token);
        return result.Succeeded;
    }

    public async Task<bool> IsExistUserAsync(string email)
    {
        return await _userManager.Users.AnyAsync(e => e.Email == email);
    }
}