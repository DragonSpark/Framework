using DragonSpark.Commands;
using DragonSpark.Diagnostics;
using DragonSpark.Sources;
using Serilog.Events;

namespace DragonSpark.Testing.Framework.Application.Setup
{
	public class MinimumLevel : CommandAttributeBase
	{
		public MinimumLevel( LogEventLevel level ) : base( MinimumLevelConfiguration.Default.Configured( level ).Adapt<AutoData>().WithPriority( Priority.BeforeNormal ) ) {}
	}
}