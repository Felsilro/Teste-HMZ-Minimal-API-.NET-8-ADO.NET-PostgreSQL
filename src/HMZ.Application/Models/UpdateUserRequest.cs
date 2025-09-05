namespace HMZ.Application.Models;

public readonly record struct UpdateUserRequest(string FirstName, string LastName, string? AvatarUrl);
