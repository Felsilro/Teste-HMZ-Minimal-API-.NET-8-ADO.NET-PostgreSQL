using HMZ.Application.Models;
using HMZ.Application.Services;
using HMZ.Domain.Repositories;

namespace HMZ.Application.Handlers;

public sealed class AuthHandler
{
    private readonly IUserRepository _users;
    private readonly JwtTokenService _jwt;
    private readonly PasswordHasher _hasher;

    public AuthHandler(IUserRepository users, JwtTokenService jwt, PasswordHasher hasher)
    {
        _users = users;
        _jwt = jwt;
        _hasher = hasher;
    }

    public async Task<LoginResponse> LoginAsync(string email, string password)
    {
        var cred = await _users.GetCredentialsByEmailAsync(email);
        if (cred is null)
            throw new UnauthorizedAccessException("Invalid email or password");

        var c = cred.Value;
        var ok = _hasher.Verify(password, c.PasswordSaltHex, c.PasswordHashHex, c.Iterations);
        if (!ok)
            throw new UnauthorizedAccessException("Invalid email or password");

        var (token, expires) = _jwt.Create(c.UserId.ToString(), email);
        return new LoginResponse(token, expires);
    }
}
