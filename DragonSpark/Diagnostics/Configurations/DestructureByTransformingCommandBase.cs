using DragonSpark.Sources.Parameterized;
using PostSharp.Patterns.Contracts;
using Serilog.Configuration;

namespace DragonSpark.Diagnostics.Configurations
{
	public abstract class DestructureByTransformingCommandBase<T> : DestructureCommandBase
	{
		protected DestructureByTransformingCommandBase( IParameterizedSource<T, object> source )
		{
			Source = source;
		}

		[Required]
		public IParameterizedSource<T, object> Source { [return: Required]get; set; }

		protected override void Configure( LoggerDestructuringConfiguration configuration ) => configuration.ByTransforming<T>( Source.Get );
	}
}