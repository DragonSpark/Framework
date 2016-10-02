using PostSharp.Patterns.Contracts;
using System.Windows.Input;
using System.Windows.Markup;

namespace DragonSpark.Commands
{
	[ContentProperty( nameof(Parameter) )]
	public class DeclaredSuppliedCommand : DeclaredCommandBase<object>
	{
		[Required]
		public ICommand Command { [return: Required]get; set; }

		public override void Execute( object parameter ) => Command.Execute( Parameter );
	}
}