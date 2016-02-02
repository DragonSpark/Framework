using DragonSpark.ComponentModel;
using DragonSpark.Extensions;
using DragonSpark.Runtime;
using PostSharp.Patterns.Contracts;

namespace DragonSpark.Setup.Commands
{
	public class DelegatedCommand<TCommand, TParameter> : SetupCommandBase where TCommand : ICommand<TParameter>
	{
		protected override void OnExecute( object parameter ) => Command.ExecuteWith( Parameter );

		[Required, Locate]
		public TCommand Command { [return: Required]get; set; }

		[Required, Locate]
		public TParameter Parameter { [return: Required]get; set; }
	}
}