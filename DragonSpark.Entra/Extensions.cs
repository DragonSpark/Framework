using DragonSpark.Application.AspNet.Compose;
using System.Security.Claims;

namespace DragonSpark.Entra;

public static class Extensions
{
	extension(AuthenticationContext @this)
	{
		public AuthenticationContext UsingEntra<T>() where T : class => @this.Append(Registrations<T>.Default);
	}

	extension(ClaimsPrincipal @this)
	{
		public string? Email() => Entra.Email.Default.Get(@this);
	}
}