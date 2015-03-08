using System;
using System.Threading;
using DragonSpark.Application.Presentation.Commands;

namespace DragonSpark.Features
{
	public class ExceptionThrowingOperation : OperationCommandBase
	{
		protected override void ExecuteCommand( ICommandMonitor monitor )
		{
			throw new InvalidOperationException( "This is an exception." );
		}
	}

	public class SampleOperation : OperationCommandBase<TimeSpan>
	{
		protected override void ExecuteCommand( ICommandMonitor parameter )
		{
			Thread.Sleep( ContextChecked );
		}
	}
}