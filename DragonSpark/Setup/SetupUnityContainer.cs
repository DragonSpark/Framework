using DragonSpark.Extensions;
using Microsoft.Practices.Unity;
using Prism;
using Prism.Unity;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace DragonSpark.Setup
{
	// [ContentProperty( "Types" )]
	public class SetupUnityContainer : SetupCommand
	{
		public Collection<UnityContainerExtension> Extensions
		{
			get { return extensions; }
		}	readonly Collection<UnityContainerExtension> extensions = new Collection<UnityContainerExtension>();

		public Collection<UnityType> Types
		{
			get { return types; }
		}	readonly Collection<UnityType> types = new Collection<UnityType>();

		public Collection<UnityInstance> Instances
		{
			get { return instances; }
		}	readonly Collection<UnityInstance> instances = new Collection<UnityInstance>();

		public Collection<ICommand> PreConfigurations
		{
			get { return preConfigurations; }
		}	readonly Collection<ICommand> preConfigurations = new Collection<ICommand>();

		public Collection<ICommand> Configurations
		{
			get { return configurations; }
		}	readonly Collection<ICommand> configurations = new Collection<ICommand>();

		public Collection<ICommand> PostConfigurations
		{
			get { return postConfigurations; }
		}	readonly Collection<ICommand> postConfigurations = new Collection<ICommand>();

		protected override void Execute( SetupContext context )
		{
			var container = context.Container();
			Extensions.Apply( x => container.AddExtension( x ) );

			var commands = PreConfigurations.Concat( Instances ).Concat( Configurations ).Concat( Types ).Concat( PostConfigurations ).ToArray();
			commands.Where( command => command.CanExecute( context ) ).Apply( item => item.Execute( context ) );
		}
	}
}