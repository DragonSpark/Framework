using DragonSpark.Sources.Scopes;

namespace DragonSpark.Application
{
	public sealed class ApplicationServices : Scope<IApplication>
	{
		public static ApplicationServices Default { get; } = new ApplicationServices();
		ApplicationServices() {}
	}
}