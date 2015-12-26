using DragonSpark.Extensions;
using DragonSpark.Runtime;
using System.Linq;
using System.Windows.Markup;

namespace DragonSpark.Setup
{
	[ContentProperty( nameof(Commands) )]
	public class Setup : ISetup
	{
		public Collection<object> Items { get; } = new Collection<object>();

		public CommandCollection Commands { get; } = new CommandCollection();
		
		public virtual void Run( ISetupParameter parameter )
		{
			parameter.Register<ISetup>( this );

			Commands
				.Where( command => command.CanExecute( parameter ) )
				.Each( command => command.Execute( parameter ) );
		}
	}
}
