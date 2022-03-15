namespace DragonSpark.Application.Security.Identity.Model;

public sealed record Challenging(string Provider, string ReturnUrl);