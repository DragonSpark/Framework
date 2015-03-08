using System;
using System.ServiceModel.DomainServices.Client;
using System.Threading;
using DragonSpark.Application.Presentation.Commands;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.Entity.Operations
{
	public abstract class EntityOperationCommmandBase : EntityOperationCommmandBase<object>
	{}

	public abstract class EntityOperationCommmandBase<TParameter> : OperationCommandBase<TParameter>
	{
		readonly ManualResetEvent reset = new ManualResetEvent( false );

		protected override void OnReset( EventArgs args )
		{
			base.OnReset( args );
			reset.Reset();
		}

		protected override void Execute( ICommandMonitor monitor )
		{
			Current = ResolveOperation();
			Current.Completed += OnComplete;

			base.Execute( monitor );
		}

		protected override void ExecuteCommand( ICommandMonitor monitor )
		{
			reset.WaitOne();
		}

		OperationBase Current { get; set; }

		protected abstract OperationBase ResolveOperation();

		void OnComplete( object sender, EventArgs e )
		{
			Current.Completed -= OnComplete;
			MarkComplete( Current );
		}

		protected virtual void MarkComplete( OperationBase operation )
		{
			operation.HasError.IsTrue( operation.MarkErrorAsHandled );
			Exception = operation.Error;
			Current = null;
			Continue();
		}

		protected virtual void Continue()
		{
			reset.Set();
		}

		public void Cancel()
		{
			Current.NotNull( x => x.Cancel() );
		}
	}
}