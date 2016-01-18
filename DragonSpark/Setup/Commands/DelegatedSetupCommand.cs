using DragonSpark.ComponentModel;
using DragonSpark.Extensions;
using DragonSpark.Runtime;
using PostSharp.Patterns.Contracts;

namespace DragonSpark.Setup.Commands
{
	public class DelegatedSetupCommand<TCommand, TParameter> : SetupCommand where TCommand : ICommand<TParameter>
	{
		protected override void OnExecute( ISetupParameter parameter ) => Command.ExecuteWith( Parameter );

		[Required, Locate]
		public TCommand Command { [return: Required]get; set; }

		[Required, Locate]
		public TParameter Parameter { [return: Required]get; set; }
	}
}