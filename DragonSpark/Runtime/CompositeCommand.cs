using DragonSpark.Extensions;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
using System.Windows.Markup;

namespace DragonSpark.Runtime
{
	public class CompositeCommand : CompositeCommand<object>
	{
		public CompositeCommand( [Required]params ICommand<object>[] commands ) : base( commands ) {}
	}

	[ContentProperty( nameof(Commands) )]
	public class CompositeCommand<TParameter> : Command<TParameter>
	{
		public CompositeCommand( [Required]params ICommand<TParameter>[] commands )
		{
			Commands = new CommandCollection( commands );
		}

		public CommandCollection Commands { get; }

		protected override void OnExecute( TParameter parameter ) => Commands.ExecuteWith<ICommand>( parameter );
	}
}