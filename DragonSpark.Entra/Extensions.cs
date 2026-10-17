using DragonSpark.Application.AspNet.Compose;

namespace DragonSpark.Entra;

public static class Extensions
{
	extension(AuthenticationContext @this)
	{
		public AuthenticationContext UsingEntra<T>() where T : class => @this.Append(Registrations<T>.Default);
	}
}