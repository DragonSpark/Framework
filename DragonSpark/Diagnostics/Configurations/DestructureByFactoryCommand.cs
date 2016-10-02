using DragonSpark.Sources.Parameterized;
using Serilog.Configuration;

namespace DragonSpark.Diagnostics.Configurations
{
	public abstract class DestructureByFactoryCommand<TParameter> : DestructureCommandBase
	{
		protected DestructureByFactoryCommand( IParameterizedSource<TParameter, object> factory )
		{
			Factory = factory;
		}

		public IParameterizedSource<TParameter, object> Factory { get; set; }

		protected override void Configure( LoggerDestructuringConfiguration configuration ) => configuration.ByTransforming( Factory.ToSourceDelegate() );
	}
}