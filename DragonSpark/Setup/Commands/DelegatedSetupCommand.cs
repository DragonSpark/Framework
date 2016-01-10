using DragonSpark.ComponentModel;
using DragonSpark.Extensions;
using DragonSpark.Runtime;
using DragonSpark.Setup.Registration;
using PostSharp.Patterns.Contracts;

namespace DragonSpark.Setup.Commands
{
	public class DelegatedSetupCommand<TCommand, TParameter> : SetupCommand where TCommand : ICommand<TParameter>
	{
		protected override void OnExecute( ISetupParameter parameter ) => Command.ExecuteWith( Parameter );

		[Required, Activate]
		public TCommand Command { [return: Required]get; set; }

		[Required, Factory]
		public TParameter Parameter { [return: Required]get; set; }
	}
}