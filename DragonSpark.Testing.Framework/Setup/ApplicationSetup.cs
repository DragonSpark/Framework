using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using DragonSpark.Runtime;
using DragonSpark.Runtime.Values;
using DragonSpark.Setup;
using DragonSpark.TypeSystem;

namespace DragonSpark.Testing.Framework.Setup
{
	public class ApplicationSetup<TLogger, TAssemblyProvider> : DragonSpark.Setup.Commands.ApplicationSetup<TLogger, TAssemblyProvider, AutoData>
		where TLogger : IMessageLogger
		where TAssemblyProvider : IAssemblyProvider
	{
		protected override void OnExecute( ISetupParameter<AutoData> parameter )
		{
			using ( var command = new SetupContextCommand() )
			{
				command.Apply( parameter.Arguments );

				base.OnExecute( parameter );
			}
		}
	}

	public class SetupAutoDataContext : ExecutionContextValue<AutoData>
	{}

	public class SetupContextCommand : ValueContextCommand<AutoData>
	{
		public SetupContextCommand() : this( new SetupAutoDataContext() )
		{}

		public SetupContextCommand( IWritableValue<AutoData> value ) : base( value )
		{}
	}
}