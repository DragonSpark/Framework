using System;

namespace DragonSpark.Application.Presentation.Commands
{
	public interface IOperation : IMonitoredCommand
	{
		event EventHandler<OperationCompletedEventArgs> Completed;

		void Cancel();

		void Abort();
	}
}