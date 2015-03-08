using System;
using System.Windows;
using System.Windows.Markup;
using DragonSpark.Application.Presentation.ComponentModel;
using DragonSpark.Extensions;
using DragonSpark.Runtime;

namespace DragonSpark.Application.Presentation.Commands
{
	[ContentProperty( "Context" )]
	public abstract class MonitoredCommandBase : CommandBase<ICommandMonitor>, IMonitoredCommand, IAttachedObject
	{
		public event EventHandler Executing = delegate { }, Executed = delegate { };
		readonly ViewAwareSupporter viewAwareSupporter;

		protected MonitoredCommandBase()
		{
			viewAwareSupporter = new ViewAwareSupporter( this );
		}

		void IMonitoredCommand.Reset()
		{
			OnReset( EventArgs.Empty );
		}

		protected virtual void OnReset( EventArgs args )
		{
			Status = ExecutionStatus.None;
			Exception = null;
		}

		protected override void Execute( ICommandMonitor monitor )
		{
			Status = ExecutionStatus.Executing;
			Executing( this, EventArgs.Empty );
			try
			{
				ExecuteCommand( monitor );
			}
			catch ( Exception e )
			{
				Exception = ResolveException( e );
			}
			OnExecuted( EventArgs.Empty );
		}

		protected abstract void ExecuteCommand( ICommandMonitor monitor );

		protected virtual Exception ResolveException( Exception e )
		{
			return e;
		}

		protected virtual void OnExecuted( EventArgs args )
		{
			Executed( this, EventArgs.Empty );
			var canceled = Status == ExecutionStatus.Canceled;
			Status = Exception.Transform( x => ExecutionStatus.CompletedWithException, () => canceled ? Status : ExecutionStatus.Completed );
			OnCanExecuteChanged();
		}

		public bool IsEnabled
		{
			get { return isEnabled; }
			set { SetProperty( ref isEnabled, value, () => IsEnabled ); }
		}	bool isEnabled = true;

		public object Context
		{
			get { return context; }
			set { SetProperty( ref context, value.ConvertTo( ContextType ), () => Context ); }
		}	object context;

		protected virtual Type ContextType
		{
			get { return typeof(object); }
		}

		/*public bool IsEnabled
		{
			get { return GetValue( IsEnabledProperty ).To<bool>(); }
			set { this.SetProperty( IsEnabledProperty, value ); }
		}	public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.Register( "IsEnabled", typeof(bool), typeof(MonitoredCommandBase), new PropertyMetadata( true ) );

		public object Context
		{
			get { return GetValue( ParameterProperty ).To<object>(); }
			set { this.SetProperty( ParameterProperty, value, ContextType ); }
		}	public static readonly DependencyProperty ParameterProperty = DependencyProperty.Register( "Context", typeof(object), typeof(MonitoredCommandBase), new PropertyMetadata( ( s, a ) => s.As<MonitoredCommandBase>( x => x.ParameterValue = a.NewValue ?? x.ParameterValue ) ) );*/

		/*protected object ParameterValue { get; private set; }*/

		public virtual string Title
		{
			get { return title; }
			set { SetProperty( ref title, value, () => Title ); }
		}	string title;

		public Exception Exception
		{
			get { return exception; }
			protected set { SetProperty( ref exception, value, () => Exception ); }
		}	Exception exception;

		public virtual CommandExceptionHandlingAction ExceptionHandlingAction
		{
			get { return exceptionHandlingAction; }
			set { SetProperty( ref exceptionHandlingAction, value, () => ExceptionHandlingAction ); }
		}	CommandExceptionHandlingAction exceptionHandlingAction;

		public ExecutionStatus Status
		{
			get { return status; }
			protected set { SetProperty( ref status, value, () => Status ); }
		}	ExecutionStatus status;

		#region IAttachedObject
		event EventHandler IAttachedObject.Attached
		{
			add { viewAwareSupporter.Attached += value; }
			remove { viewAwareSupporter.Attached -= value; }
		}

		event EventHandler IAttachedObject.Detached
		{
			add { viewAwareSupporter.Detached += value; }
			remove { viewAwareSupporter.Detached -= value; }
		}

		void System.Windows.Interactivity.IAttachedObject.Attach( DependencyObject dependencyObject )
		{
			viewAwareSupporter.Attach( dependencyObject );
			OnAttached();
		}

		protected virtual void OnAttached()
		{}

		void System.Windows.Interactivity.IAttachedObject.Detach()
		{
			OnDetached();
		}

		protected virtual void OnDetached()
		{}

		public DependencyObject AssociatedObject
		{
			get { return viewAwareSupporter.AssociatedObject; }
		}
		#endregion
	}
}