using System;
using System.Windows.Input;

namespace DragonSpark.Application.Presentation.Commands
{
    public interface IMonitoredCommand : ICommand
	{
		string Title { get; set; }

		bool IsEnabled { get; }

		object Context { get; }

		Exception Exception { get; }

		CommandExceptionHandlingAction ExceptionHandlingAction { get; }
		
		ExecutionStatus Status { get; }

		void Reset();
	}
}