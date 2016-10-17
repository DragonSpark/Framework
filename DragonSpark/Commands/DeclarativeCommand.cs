using PostSharp.Patterns.Contracts;
using System.Runtime.InteropServices;
using System.Windows.Input;
using System.Windows.Markup;

namespace DragonSpark.Commands
{
	[ContentProperty( nameof(Parameter) )]
	public class DeclarativeCommand : CommandBase<object>
	{
		[Required]
		public object Parameter { [return: Required]get; set; }

		[Required]
		public ICommand Command { [return: Required]get; set; }

		public override void Execute( [Optional]object parameter ) => Command.Execute( Parameter );
	}
}