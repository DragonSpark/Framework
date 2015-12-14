using System.Collections.Generic;
using DragonSpark.Extensions;
using DragonSpark.Runtime;
using System.Linq;
using System.Windows.Markup;

namespace DragonSpark.Setup
{
	public abstract class Setup : Setup<CommandCollection>
	{}

	[ContentProperty( nameof(Commands) )]
	public abstract class Setup<TCollection> : ISetup where TCollection : CommandCollection, new()
	{
	    public Collection<object> Items { get; } = new Collection<object>();

		public CommandCollection Commands { get; } = new TCollection();
		
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
