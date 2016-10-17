using PostSharp.Patterns.Contracts;
using Serilog.Configuration;
using System;

namespace DragonSpark.Diagnostics.Configurations
{
	public class DestructureByTransformCommand<T> : DestructureCommandBase
	{
		[Required]
		public Func<T, object> Transform { [return: Required]get; set; }

		protected override void Configure( LoggerDestructuringConfiguration configuration ) => configuration.ByTransforming( Transform );
	}
}