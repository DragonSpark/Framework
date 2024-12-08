namespace DragonSpark.Application.AspNet.Security.Identity.Model;

public sealed record Challenging(string Provider, string ReturnUrl);