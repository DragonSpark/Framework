using DragonSpark.Extensions;
using DragonSpark.Runtime;
using System.Linq;
using System.Windows.Markup;
using DragonSpark.ComponentModel;

namespace DragonSpark.Setup
{
	/*public class Setup<TLogging> : DragonSpark.Setup.Setup where TLogging : ILogger, new()
	{
		public Setup() : base( new CommandCollection<TLogging, AssemblyModuleCatalog>() )
		{}
	}*/

	[ContentProperty( nameof(Commands) ), BuildUp]
	public class Setup : ISetup
	{
		/*protected Setup() : this( new ICommand[0] )
		{}

		protected Setup( IEnumerable<ICommand> commands )
		{
			Commands.AddRange( commands );
		}*/

		public Collection<object> Items { get; } = new Collection<object>();

		public CommandCollection Commands { get; } = new CommandCollection();
		
		public virtual void Run( object argument = null )
		{
			using ( var context = CreateContext( argument ) )
			{
				Commands
					.Where( command => command.CanExecute( context ) )
					.Each( command => command.Execute( context ) );
			}
		}

		protected virtual SetupContext CreateContext( object arguments )
		{
			var result = new SetupContext( arguments );
			result.Register<ISetup>( this );
			return result;
		}
	}
}
