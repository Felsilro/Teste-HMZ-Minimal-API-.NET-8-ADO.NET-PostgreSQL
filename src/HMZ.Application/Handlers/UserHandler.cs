using HMZ.Application.Models;
using HMZ.Domain.Models;
using HMZ.Domain.Repositories;

namespace HMZ.Application.Handlers;

public sealed class UserHandler
{
    private readonly IUserRepository _users;
    public UserHandler(IUserRepository users) => _users = users;

    public Task<PagedResult<UserDto>> GetPagedAsync(int page, int perPage) => _users.GetPagedAsync(page, perPage);
    public Task<UserDto?> GetByIdAsync(int id) => _users.GetByIdAsync(id);
    public Task<bool> UpdateAsync(int id, UpdateUserRequest req) => _users.UpdateAsync(id, req.FirstName, req.LastName, req.AvatarUrl);
    public Task<bool> DeleteAsync(int id) => _users.DeleteAsync(id);
}
