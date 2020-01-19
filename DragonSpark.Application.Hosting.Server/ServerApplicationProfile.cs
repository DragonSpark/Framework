using DragonSpark.Server.Application;

namespace DragonSpark.Application.Hosting.Server {
	public sealed class ServerApplicationProfile : ServerProfile
	{
		public static IServerProfile Default { get; } = new ServerApplicationProfile();

		ServerApplicationProfile()
			: base(DefaultServiceConfiguration.Default.Execute, DefaultApplicationConfiguration.Default.Execute) {}
	}
}