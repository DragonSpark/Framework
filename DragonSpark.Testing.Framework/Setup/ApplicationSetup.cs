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
			using ( var command = new AssignSetupCommand() )
			{
				CommandExtensions.Apply( command, (object)parameter.Arguments );

				base.OnExecute( parameter );
			}
		}
	}

	public class SetupAutoDataContext : ExecutionContextValue<AutoData>
	{}

	public class AssignSetupCommand : AssignValueCommand<AutoData>
	{
		public AssignSetupCommand() : this( new SetupAutoDataContext() )
		{}

		public AssignSetupCommand( IWritableValue<AutoData> value ) : base( value )
		{}
	}
}