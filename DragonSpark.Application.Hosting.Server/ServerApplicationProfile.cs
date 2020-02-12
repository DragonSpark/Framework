using DragonSpark.Application.Compose;

namespace DragonSpark.Application.Hosting.Server
{
	public sealed class ServerApplicationProfile : ApplicationProfile
	{
		public static IApplicationProfile Default { get; } = new ServerApplicationProfile();

		ServerApplicationProfile()
			: base(DefaultServiceConfiguration.Default.Execute, DefaultApplicationConfiguration.Default.Execute) {}
	}
}