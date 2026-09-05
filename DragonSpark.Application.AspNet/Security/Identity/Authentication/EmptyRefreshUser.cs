using System.Security.Claims;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication;

public sealed class EmptyRefreshUser : IRefreshUser
{
	public static EmptyRefreshUser Default { get; } = new();

	EmptyRefreshUser() {}

	public ValueTask Get(ClaimsPrincipal parameter) => ValueTask.CompletedTask;
}