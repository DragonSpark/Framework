using DragonSpark.Sources.Scopes;
using Serilog.Events;

namespace DragonSpark.Diagnostics
{
	public sealed class MinimumLevelConfiguration : Scope<LogEventLevel>
	{
		public static MinimumLevelConfiguration Default { get; } = new MinimumLevelConfiguration();
		MinimumLevelConfiguration() : base( () => LogEventLevel.Information ) {}
	}
}
