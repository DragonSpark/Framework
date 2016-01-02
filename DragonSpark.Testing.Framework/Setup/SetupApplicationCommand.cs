using DragonSpark.ComponentModel;
using DragonSpark.Diagnostics;
using DragonSpark.Setup;
using DragonSpark.TypeSystem;

namespace DragonSpark.Testing.Framework.Setup
{
	public class SetupApplicationCommand<TLogger, TAssemblyProvider> : DragonSpark.Setup.Commands.SetupApplicationCommand<TLogger, TAssemblyProvider, SetupAutoDataParameter> 
		where TLogger : IMessageLogger
		where TAssemblyProvider : IAssemblyProvider
	{
		[Singleton]
		public CurrentExecution Current { get; set; }

		protected override void OnExecute( ISetupParameter<SetupAutoDataParameter> parameter )
		{
			parameter.RegisterForDispose( new ExecutionContext( Current, parameter.Arguments.Method ) );

			base.OnExecute( parameter );
		}
	}
}