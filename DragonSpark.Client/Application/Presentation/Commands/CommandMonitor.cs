using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using DragonSpark.Application.Presentation.ComponentModel;
using DragonSpark.Extensions;
using DragonSpark.IoC;
using DragonSpark.Objects;
using DragonSpark.Runtime;
using Microsoft.Practices.Prism.Commands;

namespace DragonSpark.Application.Presentation.Commands
{
	[PerRequest( typeof(ICommandMonitor), Priority = Priority.Lowest )]
	public class CommandMonitor : ViewObject, ICommandMonitor
	{
		readonly IExceptionHandler handler;
		public event EventHandler Started = delegate{}, Completed = delegate { };
		
		readonly Queue<IMonitoredCommand> remaining = new Queue<IMonitoredCommand>();

		readonly ViewCollection<IMonitoredCommand> source = new ViewCollection<IMonitoredCommand>();
		readonly ReadOnlyObservableCollection<IMonitoredCommand> commands;
		readonly DelegateCommand<Exception> displayExceptionDetails;

		public CommandMonitor( IExceptionHandler handler )
		{
			this.handler = handler;
			commands = new ReadOnlyObservableCollection<IMonitoredCommand>( source );
			displayExceptionDetails = new DelegateCommand<Exception>( x => this.handler.Handle( x ), x => x != null && Commands.Any( y => y.Exception != null ) );
		}

		public ICommand DisplayExceptionDetails
		{
			get { return displayExceptionDetails; }
		}

		void ICommandMonitor.Monitor( CommandMonitorContext context )
		{
			Monitor( context );
		}

		protected virtual void Monitor( CommandMonitorContext context )
		{
			var monitorOptions = context.Options ?? Options ?? CommandMonitorOptions.Default;
			Options = monitorOptions.Clone().WithDefaults();

			Title = Options.Title ?? Title ?? context.Commands.FirstOrDefault().Transform( x => x.Title );

			source.AddRange( context.Commands );

			context.Commands.Apply( x =>
			{
				x.Reset();
				remaining.Enqueue( x );
			} );

			CurrentCommand.Null( () =>
			{
				Started( this, EventArgs.Empty );
				Status = ExecutionStatus.Executing;
				CommandCompleted();
			} );
		}

		public IEnumerable<IMonitoredCommand> Commands
		{
			get { return commands; }
		}

		public double PercentageComplete
		{
			get { return percentageComplete; }
			private set { SetProperty( ref percentageComplete, value, () => PercentageComplete ); }
		}	double percentageComplete;

		public bool AllowClose
		{
			get { return allowClose; }
			private set { SetProperty( ref allowClose, value, () => AllowClose ); }
		}	bool allowClose = true;
		
		public ExecutionStatus Status
		{
			get { return status; }
			private set { SetProperty( ref status, value, () => Status ); }
		}	ExecutionStatus status;

		public virtual string Title
		{
			get { return title; }
			set { SetProperty( ref title, value, () => Title ); }
		}	string title;

		IMonitoredCommand CurrentCommand
		{
			get { return currentCommand; }
			set
			{
				if ( SetProperty( ref currentCommand, value, () => CurrentCommand ) )
				{
					CurrentOperation = CurrentCommand.As<IOperation>();
				}
			}
		}	IMonitoredCommand currentCommand;

		IOperation CurrentOperation
		{
			get { return currentOperation; }
			set
			{
				currentOperation.NotNull( x => x.Completed -= CommandCompleted );

				if ( SetProperty( ref currentOperation, value, () => CurrentOperation ) )
				{
					currentOperation.NotNull( x => x.Completed += CommandCompleted );
				}
			}
		}	IOperation currentOperation;

		public ICommand Close
		{
			get { return closeCommand ?? ( closeCommand = new DelegateCommand( OnClose, () => AllowClose ) ); }
		}	ICommand closeCommand;

		protected virtual void OnClose()
		{
            source.Apply( x => x.Reset() );

			Completed( this, EventArgs.Empty );
            
			source.Clear();
			remaining.Clear();

			Status = ExecutionStatus.None;
			CurrentOperation = null;
		}

		public ICommand Cancel
		{
			get { return cancelCommand ?? ( cancelCommand = new DelegateCommand( OnCancel, () => Options.AllowCancel.GetValueOrDefault() ) ); }
		}	ICommand cancelCommand;

		protected virtual void OnCancel()
		{
			Options.CloseOnCompletion = false;
			CurrentOperation.NotNull( x => x.Cancel() );
		}
	
		public CommandMonitorOptions Options
		{
			get { return options; }
			private set { SetProperty( ref options, value, () => Options ); }
		}	CommandMonitorOptions options;

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Intended to catch general exceptions." )]
		void CommandCompleted( object sender = null, OperationCompletedEventArgs e = null )
		{
			var args = e ?? OperationCompletedEventArgs.Default;

			Options.CloseOnCompletion &= args.Error == null;

			var @continue = !args.WasCancelled && ( args.Error == null || ( !Options.AnyExceptionIsFatal && CurrentCommand.Transform( x => x.ExceptionHandlingAction == CommandExceptionHandlingAction.Continue, () => true ) ) );
			if ( @continue )
			{
				PercentageComplete = ( source.IndexOf( CurrentOperation ) + 2d ) / ( source.Count + 1d );

				CurrentCommand = remaining.Any() ? remaining.Dequeue() : null;
				if ( CurrentCommand != null )
				{
					try
					{
						CurrentCommand.IsEnabled.IsTrue( () => CurrentCommand.Execute( this ) );
						var next = CurrentOperation == null || !CurrentCommand.IsEnabled;
						next.IsTrue( () => CommandCompleted() );
					}
					catch ( Exception ex )
					{
						OnCompleted( ex );
					}
				}
				else
				{
					OnCompleted();
				}
			}
			else
			{
				OnCompleted( args.Error, args.WasCancelled );
			}
		}

		protected virtual void OnCompleted( Exception exception = null, bool wasCanceled = false )
		{
			var items = CurrentCommand.Transform( x => source.Skip( source.IndexOf( x ) + 1 ) );
			items.OfType<IOperation>().Apply( x => x.Abort() );

			PercentageComplete = 1;
			Status = wasCanceled ? ExecutionStatus.Canceled : source.FirstOrDefault( x => x.Exception != null ).Transform( x => Options.AnyExceptionIsFatal || x.Exception is FatalApplicationException ? ExecutionStatus.CompletedWithFatalException : ExecutionStatus.CompletedWithException, () => ExecutionStatus.Completed );

			AllowClose = Status != ExecutionStatus.CompletedWithFatalException;
			Options.AllowCancel = false;

			var close = AllowClose && Options.CloseOnCompletion.GetValueOrDefault() && exception == null;
			close.IsTrue( OnClose );

			var propagate = CurrentCommand.Transform( x => x.Exception != null && x.ExceptionHandlingAction == CommandExceptionHandlingAction.Throw ? x.Exception : null );

			CurrentCommand = null;
			
			if ( propagate != null )
			{
				throw propagate;
			}
		}
	}
}