using DragonSpark.Extensions;
using Microsoft.Practices.Unity;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Markup;

namespace DragonSpark.Setup
{
	[ContentProperty( "Types" )]
	public class SetupUnityContainer : SetupCommand
	{
		public Collection<UnityContainerExtension> Extensions { get; } = new Collection<UnityContainerExtension>();

		public SetupCommandCollection<UnityType> Types { get; } = new SetupCommandCollection<UnityType>();

		public SetupCommandCollection<UnityInstance> Instances { get; } = new SetupCommandCollection<UnityInstance>();

		public SetupCommandCollection<ICommand> PreConfigurations { get; } = new SetupCommandCollection<ICommand>();

		public SetupCommandCollection<ICommand> Configurations { get; } = new SetupCommandCollection<ICommand>();

		public SetupCommandCollection<ICommand> PostConfigurations { get; } = new SetupCommandCollection<ICommand>();

		protected override void Execute( SetupContext context )
		{
			var container = context.Container();
			Extensions.Apply( x => container.AddExtension( x ) );

			var commands = PreConfigurations.Concat( Instances ).Concat( Configurations ).Concat( Types ).Concat( PostConfigurations ).ToArray();
			commands.Where( command => command.CanExecute( context ) ).Apply( item => item.Execute( context ) );
		}
	}
}