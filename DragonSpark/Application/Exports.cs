using DragonSpark.Sources;

namespace DragonSpark.Application
{
	public sealed class Exports : Scope<IExportProvider>
	{
		public static IScope<IExportProvider> Default { get; } = new Exports();
		Exports() : base( () => DefaultExportProvider.Default ) {}
	}
}