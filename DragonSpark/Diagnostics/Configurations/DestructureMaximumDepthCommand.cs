using PostSharp.Patterns.Contracts;
using Serilog.Configuration;

namespace DragonSpark.Diagnostics.Configurations
{
	public class DestructureMaximumDepthCommand : DestructureCommandBase
	{
		[Range( 0, 100 )]
		public int MaximumDepth { get; set; }

		protected override void Configure( LoggerDestructuringConfiguration configuration ) => configuration.ToMaximumDepth( MaximumDepth );
	}
}