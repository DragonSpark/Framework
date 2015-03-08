using System;

namespace DragonSpark.Application.Presentation.Commands
{
	public class OperationCompletedEventArgs : EventArgs
	{
		public static OperationCompletedEventArgs Default
		{
			get { return DefaultField; }
		}	static readonly OperationCompletedEventArgs DefaultField = new OperationCompletedEventArgs( null, false );

		public OperationCompletedEventArgs( Exception error, bool wasCancelled )
		{
			Error = error;
			WasCancelled = wasCancelled;
		}

		public Exception Error { get; private set; }
		public bool WasCancelled { get; private set; }
	}
}