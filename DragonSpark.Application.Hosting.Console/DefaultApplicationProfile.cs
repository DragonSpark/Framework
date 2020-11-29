using DragonSpark.Application.Compose;

namespace DragonSpark.Application.Hosting.Console
{
	sealed class DefaultApplicationProfile : ApplicationProfile
	{
		public static DefaultApplicationProfile Default { get; } = new DefaultApplicationProfile();

		DefaultApplicationProfile() : base(_ => {}, _ => {}) {}
	}
}