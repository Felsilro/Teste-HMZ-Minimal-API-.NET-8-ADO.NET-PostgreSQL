namespace HMZ.Domain.Models;

public readonly record struct UserDto(int Id, string Email, string FirstName, string LastName, string? AvatarUrl);
