using System.Composition;

namespace DragonSpark.Application.Setup
{
	[Export( typeof(ISetup) )]
	public sealed class EnableServicesCommand : DeclarativeSetup
	{
		public EnableServicesCommand() : base( Priority.High, Sources.Extensions.Configured( ServicesEnabled.Default, true ) ) {}
	}
}