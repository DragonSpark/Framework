using DragonSpark.Commands;
using DragonSpark.ComponentModel;
using DragonSpark.Sources.Parameterized;

namespace DragonSpark.Testing.Framework.Application.Setup
{
	public abstract class ExecutedRunCommandAttributeBase : HostingAttributeBase
	{
		protected ExecutedRunCommandAttributeBase( IRunCommand command ) : base( new Wrapper( command ).Get ) {}

		sealed class Wrapper : ParameterizedSourceBase<object, ICommand<AutoData>>
		{
			readonly IRunCommand command;
			public Wrapper( IRunCommand command )
			{
				this.command = command;
			}

			public override ICommand<AutoData> Get( object parameter )
			{
				command.Execute();
				var result = command.Adapt<AutoData>();
				return result;
			}
		}
	}
}