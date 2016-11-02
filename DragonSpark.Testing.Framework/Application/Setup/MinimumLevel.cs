using DragonSpark.Commands;
using DragonSpark.Diagnostics;
using DragonSpark.Sources.Scopes;
using Serilog.Events;

namespace DragonSpark.Testing.Framework.Application.Setup
{
	public class MinimumLevel : CommandAttributeBase
	{
		public MinimumLevel( LogEventLevel level ) : base( MinimumLevelConfiguration.Default.ToCommand( level ).Adapt<AutoData>().WithPriority( Priority.BeforeNormal ) ) {}
	}
}