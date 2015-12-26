using DragonSpark.ComponentModel;
using DragonSpark.Diagnostics;
using DragonSpark.Setup;
using DragonSpark.Testing.Framework.Extensions;
using DragonSpark.TypeSystem;

namespace DragonSpark.Testing.Framework.Setup
{
	public class SetupApplicationCommand<TLogger, TAssemblyProvider> : DragonSpark.Setup.Commands.SetupApplicationCommand<TLogger, TAssemblyProvider> 
		where TLogger : IMessageLogger
		where TAssemblyProvider : IAssemblyProvider
	{
		[Singleton]
		public CurrentExecution Current { get; set; }

		protected override void Execute( ISetupParameter parameter )
		{
			parameter.RegisterForDispose( new ExecutionContext( Current, parameter.Method() ) );

			base.Execute( parameter );
		}
	}
}