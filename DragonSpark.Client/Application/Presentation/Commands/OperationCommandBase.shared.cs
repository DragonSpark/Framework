using System;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.Commands
{
	public abstract class OperationCommandBase : OperationCommandBase<object>
	{}

	public abstract class OperationCommandBase<TParameter> : MonitoredCommandBase<TParameter>, IOperation
	{
		public event EventHandler<OperationCompletedEventArgs> Completed = delegate {};
		
		TaskContext Task { get; set; }

		protected override void OnReset( EventArgs args )
		{
			base.OnReset( args );

			Task = null;
		}

		protected override bool CanExecute( ICommandMonitor monitor )
		{
			var result = base.CanExecute( monitor ) && Status == ExecutionStatus.None;
			return result;
		}

		protected override void Execute( ICommandMonitor monitor )
		{
			Task = Threading.Background.ExecuteAs<TaskContext>( () => base.Execute( monitor ) ).To<TaskContext>();
		}

		void IOperation.Abort()
		{
			OnAbort( EventArgs.Empty );
		}

		protected virtual void OnAbort( EventArgs args )
		{
			Status = ExecutionStatus.Aborted;
		}

		protected override void OnExecuted( EventArgs args )
		{
			Threading.Application.Execute( () =>
			{
				base.OnExecuted( args );
				Completed( this, new OperationCompletedEventArgs( Exception, Status == ExecutionStatus.Canceled ) );
			} );
		}

		void IOperation.Cancel()
		{
			Task.NotNull( x => x.Cancel() );
			OnCancel( EventArgs.Empty );
		}

		protected virtual void OnCancel( EventArgs args )
		{
			Status = ExecutionStatus.Canceled;
			OnExecuted( EventArgs.Empty );
		}
	}
}