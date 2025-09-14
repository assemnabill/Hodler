using Corz.DomainDriven.Abstractions.Models.Bases;
using Hodler.Domain.Users.Models;
using Hodler.Domain.Users.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace Hodler.Domain.Users.Ports;

public interface IUserRepository : IRepository<IUser>
{
    Task<IUser?> FindByAsync(
        UserId userId,
        CancellationToken cancellationToken
    );
    Task<IUser?> FindByEmailAsync(EmailAddress email, CancellationToken cancellationToken);
    Task<IUser?> FindByUserNameAsync(UserName userName, CancellationToken cancellationToken);
    Task<bool> IsExistUserAsync(UserName? userName, EmailAddress email, CancellationToken cancellationToken);
    Task<bool> IsExistUserAsync(EmailAddress email, CancellationToken cancellationToken);
    Task<IdentityResult> CreateAsync(IUser user, string password, CancellationToken cancellationToken);
    Task<IUser?> CheckLoginCredentialsAsync(UserName userName, string password, CancellationToken cancellationToken);
    Task<IUser?> CheckLoginCredentialsAsync(EmailAddress email, string password, CancellationToken cancellationToken);
    Task<bool> ResetPasswordAsync(EmailAddress email, string newPassword, string token);
    Task<string> GenerateResetPasswordTokenAsync(EmailAddress email);
    Task<string> GenerateConfirmEmailTokenAsync(EmailAddress email);
    Task<bool> ConfirmEmailAsync(EmailAddress email, string token);
    Task StoreRefreshTokenAsync(string refreshToken, DateTime expiryDate, UserId userId ,CancellationToken cancellationToken);
    Task<bool> IsRefreshTokenValidAsync(string refreshToken, UserId userId ,CancellationToken cancellationToken);
    Task<bool> UpdateRefreshTokenAsync(string oldRefreshToken, string newRefreshToken,UserId userId ,DateTime expiryDate ,CancellationToken cancellationToken);
    Task<bool> DeleteRefreshTokenAsync(string refreshToken, UserId userId , CancellationToken cancellationToken);
}