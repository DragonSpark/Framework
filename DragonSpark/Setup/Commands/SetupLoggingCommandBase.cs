using DragonSpark.ComponentModel;
using DragonSpark.Diagnostics;
using DragonSpark.Properties;
using PostSharp.Patterns.Contracts;

namespace DragonSpark.Setup.Commands
{
	public class SetupLoggingCommand : SetupCommand
	{
		[Required( /*ErrorMessage = Resources.NullLoggerFacadeException*/ ), Activate( typeof(MessageLogger) )]
		public IMessageLogger MessageLogger { [return: NotNull] get; set; }
		
		protected override void Execute( SetupContext context )
		{
			context.Register( MessageLogger ).Information( Resources.LoggerCreatedSuccessfully, Priority.Low );
		}
	}
}