using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Markup;
using DragonSpark.Extensions;
using Microsoft.Practices.Unity;

namespace DragonSpark.Setup.Commands
{
	[ContentProperty( nameof(Types) )]
	public class SetupUnityContainerCommand : SetupCommand
	{
		public Collection<UnityContainerExtension> Extensions { get; } = new Collection<UnityContainerExtension>();

		public CommandCollection Types { get; } = new CommandCollection();

		public CommandCollection Instances { get; } = new CommandCollection();

		public CommandCollection PreConfigurations { get; } = new CommandCollection();

		public CommandCollection Configurations { get; } = new CommandCollection();

		public CommandCollection PostConfigurations { get; } = new CommandCollection();

		protected override void Execute( SetupContext context )
		{
			var container = context.Container();
			Extensions.Each( x => container.AddExtension( x ) );

			var commands = PreConfigurations.Concat( Instances ).Concat( Configurations ).Concat( Types ).Concat( PostConfigurations ).ToArray();
			commands.Where( command => command.CanExecute( context ) ).Each( item => item.Execute( context ) );
		}
	}
}