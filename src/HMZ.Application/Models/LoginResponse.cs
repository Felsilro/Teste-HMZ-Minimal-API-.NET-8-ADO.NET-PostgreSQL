namespace HMZ.Application.Models;

public readonly record struct LoginResponse(string Token, DateTime ExpiresAtUtc);
