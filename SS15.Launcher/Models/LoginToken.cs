namespace SS15.Launcher.Models;

public readonly record struct LoginToken(string Token, DateTimeOffset ExpireTime);
