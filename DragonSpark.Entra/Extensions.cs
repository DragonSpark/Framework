using DragonSpark.Application.AspNet.Compose;

namespace DragonSpark.Entra;

public static class Extensions
{
	extension(AuthenticationContext @this)
	{
		public AuthenticationContext UsingEntra() => @this.Append(Registrations.Default);
	}
}