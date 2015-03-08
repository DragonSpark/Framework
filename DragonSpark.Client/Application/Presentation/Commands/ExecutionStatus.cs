using System.ComponentModel;

namespace DragonSpark.Application.Presentation.Commands
{
	public enum ExecutionStatus
	{
		None,

		Executing,

		Canceling,

		[Description( "Canceled by User" )]
		Canceled,

		Aborted,

		Completed,

		[Description( "Completed with Exception" )]
		CompletedWithException,

		[Description( "Completed with Fatal Exception (oops!)" )]
		CompletedWithFatalException
	}
}