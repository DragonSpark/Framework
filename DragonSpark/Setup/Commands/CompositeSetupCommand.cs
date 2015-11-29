using System.Linq;
using System.Windows.Markup;
using DragonSpark.Extensions;

namespace DragonSpark.Setup.Commands
{
	[ContentProperty( "Commands" )]
	public class CompositeSetupCommand : SetupCommand
	{
		public CommandCollection Commands { get; } = new CommandCollection();

		public override bool CanExecute( object parameter )
		{
			return base.CanExecute( parameter ) && Commands.All( command => command.CanExecute( parameter ) );
		}

		protected override void Execute( SetupContext context )
		{
			Commands.Each( command => command.Execute( context ) );
		}
	}
}