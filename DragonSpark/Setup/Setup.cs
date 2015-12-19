using DragonSpark.Extensions;
using DragonSpark.Runtime;
using System.Linq;
using System.Windows.Markup;
using DragonSpark.Aspects;
using DragonSpark.ComponentModel;
using DragonSpark.Diagnostics;
using DragonSpark.Properties;
using PostSharp.Patterns.Contracts;

namespace DragonSpark.Setup
{
	[ContentProperty( nameof(Commands) ), BuildUp]
	public class Setup : ISetup
	{
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
