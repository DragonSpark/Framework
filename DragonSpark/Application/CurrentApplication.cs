using DragonSpark.Sources.Scopes;

namespace DragonSpark.Application
{
	public sealed class CurrentApplication : Scope<IApplication>
	{
		public static CurrentApplication Default { get; } = new CurrentApplication();
		CurrentApplication() {}
	}
}