using DragonSpark.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Windows.Markup;

namespace DragonSpark.Runtime
{
	public class CompositeCommand : CompositeCommand<object>
	{
		public CompositeCommand()
		{}

		public CompositeCommand( IEnumerable<System.Windows.Input.ICommand> commands ) : base( commands )
		{}
	}

	[ContentProperty( nameof(Commands) )]
	public class CompositeCommand<TParameter> : Command<TParameter>
	{
		public CompositeCommand() : this( Enumerable.Empty<System.Windows.Input.ICommand>() )
		{}

		public CompositeCommand( IEnumerable<System.Windows.Input.ICommand> commands )
		{
			Commands = new CommandCollection( commands );
		}

		public CommandCollection Commands { get; }

		protected override void OnExecute( TParameter parameter )
		{
			 Commands.Apply<ICommand>( parameter );
		}
	}
}