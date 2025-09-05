using HMZ.Domain.Models;

namespace HMZ.Domain.Repositories;

public readonly record struct UserCredentials(int UserId, string PasswordSaltHex, string PasswordHashHex, int Iterations);

public interface IUserRepository
{
    Task<UserCredentials?> GetCredentialsByEmailAsync(string email);
    Task<PagedResult<UserDto>> GetPagedAsync(int page, int perPage);
    Task<UserDto?> GetByIdAsync(int id);
    Task<bool> UpdateAsync(int id, string firstName, string lastName, string? avatarUrl);
    Task<bool> DeleteAsync(int id);
}
