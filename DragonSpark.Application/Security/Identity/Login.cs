using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.Security.Identity
{
	public readonly record struct Login<T>(ExternalLoginInfo Information, T User);
}