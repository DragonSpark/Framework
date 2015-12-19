using DragonSpark.Extensions;
using Microsoft.Practices.Unity;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Markup;

namespace DragonSpark.Setup.Commands
{
	[ContentProperty( nameof(Instances) )]
	public class ConfigureUnityCommand : UnityCommand
	{
		public Collection<UnityContainerExtension> Extensions { get; } = new Collection<UnityContainerExtension>();

		public CommandCollection<UnityType> Types { get; } = new CommandCollection<UnityType>();

		public CommandCollection<UnityInstance> Instances { get; } = new CommandCollection<UnityInstance>();

		public CommandCollection PreConfigurations { get; } = new CommandCollection();

		public CommandCollection Configurations { get; } = new CommandCollection();

		public CommandCollection PostConfigurations { get; } = new CommandCollection();

		protected override void Execute( SetupContext context )
		{
			Extensions.Each( x => Container.AddExtension( x ) );

			var commands = PreConfigurations.Concat( Instances ).Concat( Configurations ).Concat( Types ).Concat( PostConfigurations ).ToArray();
			commands.Where( command => command.CanExecute( context ) ).Each( item => item.Execute( context ) );
		}
	}
}