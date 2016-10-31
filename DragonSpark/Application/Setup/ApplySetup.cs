using DragonSpark.Activation.Location;
using DragonSpark.Commands;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Setup
{
	public class ApplySetup : SuppliedCommand<IExportProvider>
	{
		public static ApplySetup Default { get; } = new ApplySetup();
		ApplySetup() : base( ApplyExportsCommand<ISetup>.Default, GlobalServiceProvider.Default.Get<IExportProvider> ) {}
	}
}