namespace HMZ.Domain.Entities;

public readonly record struct User(int Id, string Email, string FirstName, string LastName, string? AvatarUrl, DateTime CreatedAt);
