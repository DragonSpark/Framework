using DragonSpark.Activation;
using DragonSpark.Application.Client.Extensions;
using DragonSpark.Application.Client.Presentation;
using DragonSpark.Application.Client.Threading;
using DragonSpark.Application.Setup;
using DragonSpark.Client;
using DragonSpark.ComponentModel;
using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using DragonSpark.Setup;
using Prism.Presentation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Interactivity;
using System.Windows.Markup;
using Microsoft.Practices.Unity;
using Prism.Events;
using IAttachedObject = DragonSpark.Application.Client.Presentation.IAttachedObject;

namespace DragonSpark.Application.Client.Commanding
{
	public static class CommandExtensions
	{
		public static void Update( this System.Windows.Input.ICommand @this )
		{
			@this.As<DelegateCommandBase>( x => x.RaiseCanExecuteChanged() )
				.Null( () => @this.As<ICommand>( x => x.Refresh() ) );
		}
	}

	public interface ICommand : System.Windows.Input.ICommand
	{
		void Refresh();
	}

	public abstract class CommandBase<TParameter> : ViewObject, ICommand
	{
		public event EventHandler CanExecuteChanged = delegate { };

		public void Refresh()
		{
			OnCanExecuteChanged();
		}

		protected virtual void OnCanExecuteChanged()
		{
			CanExecuteChanged( this, EventArgs.Empty );
		}

		void System.Windows.Input.ICommand.Execute( object parameter )
		{
			var ensured = parameter.ConvertTo<TParameter>();
			Execute( ensured );
		}

		protected abstract void Execute( TParameter parameter );

		bool System.Windows.Input.ICommand.CanExecute( object parameter )
		{
			var ensured = parameter.ConvertTo<TParameter>();
			var result = CanExecute( ensured );
			return result;
		}

		protected virtual bool CanExecute( TParameter parameter )
		{
			return true;
		}
	}

	public class CommandCompletedEventArgs : EventArgs
	{
		public static CommandCompletedEventArgs Default
		{
			get { return DefaultField; }
		}

		static readonly CommandCompletedEventArgs DefaultField = new CommandCompletedEventArgs( null, false );

		public CommandCompletedEventArgs( Exception error, bool wasCancelled )
		{
			Error = error;
			WasCancelled = wasCancelled;
		}

		public Exception Error { get; private set; }
		public bool WasCancelled { get; private set; }
	}

	public interface ICommandMonitor : INotifyPropertyChanged
	{
		event EventHandler Started , Completed;

		void Monitor( CommandMonitorContext context );

		IEnumerable<ICommandModel> Items { get; }
	}

	public enum CommandExceptionHandlingAction
	{
		None,
		Continue,
		Throw
	}

	public enum ExecutionStatus
	{
		None,

		Executing,

		Canceling,

		[Description( "Canceled by User" )] Canceled,

		Completed,

		[Description( "Completed with Exception" )] CompletedWithException,

		[Description( "Completed with Fatal Exception (oops!)" )] CompletedWithFatalException
	}

	public class CommandMonitorContext
	{
		readonly IEnumerable<ICommandModel> items;
		readonly CommandMonitorOptions options;

		public CommandMonitorContext( IEnumerable<ICommandModel> items, CommandMonitorOptions options = null )
		{
			this.items = items.Select( x => x.WithDefaults() ).ToArray();
			this.options = options;
		}

		public IEnumerable<ICommandModel> Items
		{
			get { return items; }
		}

		public CommandMonitorOptions Options
		{
			get { return options; }
		}
	}

	[ContentProperty( "Items" )]
	public class MonitorCommand : ViewCommandBase<object>
	{
		readonly ICommandRegistry registry;
		public event EventHandler Completed = delegate { };

		public MonitorCommand( ICommandRegistry registry )
		{
			this.registry = registry;

			Items.CollectionChanged += ( sender, args ) =>
			{
				switch ( args.Action )
				{
					case NotifyCollectionChangedAction.Add:
						args.NewItems.OfType<ICommandModel>().Apply( x => new CommandMonitor( this, x ).Start() );
						break;
					case NotifyCollectionChangedAction.Remove:
						args.OldItems.OfType<ICommandModel>().Apply( x => x.Command.CanExecuteChanged -= XOnCanExecuteChanged );
						break;
				}
			};
			Options = CommandMonitorOptions.Default;
		}

		class CommandMonitor
		{
			readonly MonitorCommand owner;
			readonly ICommandModel model;

			public CommandMonitor( MonitorCommand owner, ICommandModel model )
			{
				this.owner = owner;
				this.model = model;
			}

			public void Start()
			{
				if ( !Check() )
				{
					model.As<INotifyPropertyChanged>( x => x.PropertyChanged += Changed );
				}
			}

			bool Check()
			{
				if ( model.Command != null )
				{
					model.Command.CanExecuteChanged += owner.XOnCanExecuteChanged;
					return true;
				}
				return false;
			}

			void Changed( object sender, PropertyChangedEventArgs e )
			{
				switch ( e.PropertyName )
				{
					case "Command":
						if ( Check() )
						{
							model.As<INotifyPropertyChanged>( x => x.PropertyChanged -= Changed );
						}
						break;
				}
			}
		}

		void XOnCanExecuteChanged( object sender, EventArgs eventArgs )
		{
			sender.As<System.Windows.Input.ICommand>( x => Items.FirstOrDefault( y => y.Command == x ).Null( () => x.CanExecuteChanged -= XOnCanExecuteChanged ) );
			Refresh();
		}

		public CommandMonitorOptions Options { get; set; }

		public IObservableCollection<ICommandModel> Items
		{
			get { return items; }
		}	readonly IObservableCollection<ICommandModel> items = new ViewCollection<ICommandModel>();

		protected override bool CanExecute( object parameter )
		{
			var result = Current == null && base.CanExecute( parameter ) && Items.All( x => x.CanExecute( parameter ) );
			return result;
		}

		ICommandMonitor Current
		{
			get { return current; }
			set
			{
				current.With( x =>
				{
					x.Completed -= OnCompleted;
				} );

				if ( current != value )
				{
					current = value;
					current.With( x =>
					{
						current.Completed += OnCompleted;
					} );
				}
			}
		}

		ICommandMonitor current;

		protected override void Execute( object parameter )
		{
			Current = parameter as ICommandMonitor ?? registry.GetMonitor( AssociatedObject );
			var context = new CommandMonitorContext( Items, Options );
			Current.Monitor( context );
		}

		void OnCompleted( object sender, EventArgs eventArgs )
		{
			Completed( this, EventArgs.Empty );
			var refresh = Current;
			Current = null;
			Commands.Refresh( refresh );
		}

		protected override void OnAttached()
		{
			base.OnAttached();
			Items.OfType<IAttachedObject>().Apply( x => x.Attach( AssociatedObject ) );
		}

		protected override void OnDetached()
		{
			base.OnDetached();
			Items.OfType<IAttachedObject>().Apply( x => x.Detach() );
		}
	}

	public interface ICommandModel
	{
		event EventHandler Executing;

		event EventHandler<CommandCompletedEventArgs> Executed;

		string Title { get; set; }

		bool IsEnabled { get; }

		object Parameter { get; set; }

		Exception Exception { get; }

		CommandExceptionHandlingAction ExceptionHandlingAction { get; }

		ExecutionStatus Status { get; }

		void Reset();

		void Cancel();

		void Execute();

		System.Windows.Input.ICommand Command { get;  }
	}

	public abstract class ViewCommandBase<TParameter> : CommandBase<TParameter>, Presentation.IAttachedObject
	{
		readonly ViewAwareSupporter viewAwareSupporter;

		protected ViewCommandBase()
		{
			viewAwareSupporter = new ViewAwareSupporter( this );
		}

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

		public void Attach( DependencyObject dependencyObject )
		{
			viewAwareSupporter.Attach( dependencyObject );
			OnAttached();
		}

		protected virtual void OnAttached()
		{
			/*var allPropertyValuesOf = this.GetAllPropertyValuesOf<IAttachedObject>();
			allPropertyValuesOf.Apply( x => x.Attach( AssociatedObject ) );*/
		}

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

	[ContentProperty( "Command" ), LifetimeManager( typeof(TransientLifetimeManager) )]
	public class CommandModel : ViewObject, ICommandModel
	{
		public event EventHandler Executing = delegate { };
		public event EventHandler<CommandCompletedEventArgs> Executed = delegate {};

		void ICommandModel.Reset()
		{
			OnReset( EventArgs.Empty );
		}

		public void Execute()
		{
			if ( IsEnabled )
			{
				Status = ExecutionStatus.Executing;
				Executing( this, EventArgs.Empty );
				ExecuteBody();
			}
		}

		protected virtual void ExecuteBody()
		{
			try
			{
				ExecuteBodyCore();
			}
			catch ( Exception e )
			{
				Handle( e );
			}
			Complete();
		}

		protected virtual void Complete()
		{
			switch ( Status )
			{
				case ExecutionStatus.Canceling:
				case ExecutionStatus.Executing:
					var canceled = Status == ExecutionStatus.Canceling;
					Status = Exception.Transform( x => ExecutionStatus.CompletedWithException, () => canceled ? ExecutionStatus.Canceled : ExecutionStatus.Completed );
					var args = new CommandCompletedEventArgs( Exception, canceled );
					Executed( this, args );
					break;
			}
		}

		protected virtual void ExecuteBodyCore()
		{
			Command.Execute( Parameter );
		}

		protected virtual void Handle( Exception e )
		{
			Exception = e;
		}

		protected virtual void OnReset( EventArgs args )
		{
			Status = ExecutionStatus.None;
			Exception = null;
		}

		public virtual System.Windows.Input.ICommand Command
		{
			get { return command; }
			set { SetProperty( ref command, value, () => Command ); }
		}	System.Windows.Input.ICommand command;

		protected virtual void OnAbort( EventArgs args )
		{}

		void ICommandModel.Cancel()
		{
			switch ( Status )
			{
				case ExecutionStatus.Executing:
					OnCancel( EventArgs.Empty );
					break;
				default:
					OnAbort( EventArgs.Empty );
					break;
			}
		}

		protected virtual void OnCancel( EventArgs args )
		{
			Status = ExecutionStatus.Canceling;
			Complete();
		}

		public bool IsEnabled
		{
			get { return isEnabled; }
			set { SetProperty( ref isEnabled, value, () => IsEnabled ); }
		}	bool isEnabled = true;

		public object Parameter
		{
			get { return parameter; }
			set { SetProperty( ref parameter, value, () => Parameter ); }
		}	object parameter;

		public virtual string Title
		{
			get { return title; }
			set { SetProperty( ref title, value, () => Title ); }
		}	string title;

		public object Content
		{
			get { return content; }
			protected set { SetProperty( ref content, value, () => Content ); }
		}	object content;

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
	}

	/*public interface ITaskProgress
	{
		void Step();
	}*/

	/*public interface IProgressAware
	{
		void Assign( IProgress progress );
	}

	public interface IProgress
	{
		int Next();

		void Total( int total );
	}*/

	public class OperationProgress : ViewObject
	{
		public OperationProgress( int total = 100 )
		{
			Total = total;
		}

		public int Next()
		{
			var result = Current++;
			return result;
		}

		public int Current
		{
			get { return current; }
			private set { SetProperty( ref current, Math.Min( value, Total ), () => Current ); }
		}	int current;

		public int Total
		{
			get { return total; }
			private set { SetProperty( ref total, value, () => Total ); }
		}	int total;
	}

	[ContentProperty( "Command" )]
	public class Operation : CommandModel
	{
		CancellationTokenSource Source { get; set; }

		protected override void OnReset( EventArgs args )
		{
			Progress = null;
			Source = new CancellationTokenSource();
			// ProgressAware.With( x => x.Assign( this ) );
			base.OnReset( args );
		}

		/*public override System.Windows.Input.ICommand Command
		{
			get { return base.Command; }
			set
			{
				base.Command = value;
				ProgressAware = value.As<IProgressAware>();
			}
		}*/

		// internal IProgressAware ProgressAware { private get; set; }

		protected override async void ExecuteBody()
		{
			try
			{
				var task = DetermineTask( base.ExecuteBodyCore, Source.Token );
				await task;
			}
			catch ( OperationCanceledException )
			{
			}
			catch ( Exception exception )
			{
				Handle( exception );
			}
			Complete();
		}

		OperationProgress Progress
		{
			get { return Content.To<OperationProgress>(); }
			set { Content = value; }
		}

		protected override void OnCancel( EventArgs args )
		{
			Source.Cancel();
			base.OnCancel( EventArgs.Empty );
		}

		protected virtual Task DetermineTask( Action execute, CancellationToken token )
		{
			return Task.Factory.StartNew( execute, token );
		}

		public int Next()
		{
			Source.Token.ThrowIfCancellationRequested();
			return Progress.Next();
		}

		public void Total( int total )
		{
			Progress = new OperationProgress( total );
		}
	}

	public static class CommandItemExtensions
	{
		public static bool CanExecute( this ICommandModel @this, object parameter )
		{
			var result = @this.Status == ExecutionStatus.None && @this.Command.Transform( x => x.CanExecute( @this.Parameter ) || Check( x, parameter ) );
			return result;
		}

		static bool Check( System.Windows.Input.ICommand command, object parameter )
		{
			try
			{
				return command.CanExecute( parameter );
			}
			catch ( InvalidCastException )
			{}
			return false;
		}
	}

	public class CommandMonitorOptions : ViewObject
	{
		public static CommandMonitorOptions Default
		{
			get { return DefaultField; }
		}	static readonly CommandMonitorOptions DefaultField = new CommandMonitorOptions().WithDefaults();

		public string Title
		{
			get { return title; }
			set { SetProperty( ref title, value, () => Title ); }
		}	string title;

		public bool AllowCancel
		{
			get { return allowCancel; }
			set { SetProperty( ref allowCancel, value, () => AllowCancel ); }
		}	bool allowCancel;

		[Default( true )]
		public bool CloseOnCompletion
		{
			get { return closeOnCompletion; }
			set { SetProperty( ref closeOnCompletion, value, () => CloseOnCompletion ); }
		}	bool closeOnCompletion;

		public bool AnyExceptionIsFatal
		{
			get { return anyExceptionIsFatal; }
			set { SetProperty( ref anyExceptionIsFatal, value, () => AnyExceptionIsFatal ); }
		}	bool anyExceptionIsFatal;
	}

	public interface ICommandRegistry
	{
		void Register( ICommandMonitor monitor, ICommand command, DependencyObject context );

		void Refresh( ICommandMonitor monitor );

		ICommandMonitor GetMonitor( DependencyObject element );
	}

	public class CommandExtension : CommandExtension<CommandModel>
	{
		[InjectionConstructor]
		public CommandExtension()
		{}

		public CommandExtension( System.Windows.Input.ICommand command ) : base( command )
		{}

		public CommandExtension( Binding binding ) : base( binding )
		{}
	}

	public class OperationExtension : CommandExtension<Operation>
	{
		[InjectionConstructor]
		public OperationExtension()
		{}

		public OperationExtension( System.Windows.Input.ICommand command ) : base( command )
		{}

		public OperationExtension( Binding binding ) : base( binding )
		{}

		/*public IProgressAware ProgressAware { get; set; }

		protected override CommandModel DetermineItem( DependencyObject owner )
		{
			var result = base.DetermineItem( owner ).As<Operation>( x =>
			{
				x.ProgressAware = ProgressAware;
			} );
			return result;
		}*/
	}

	[ContentProperty( "Command" )]
	public abstract class CommandExtension<T> : CommandBaseExtension where T : CommandModel, new()
	{
		readonly Binding binding;

		protected CommandExtension()
		{}

		protected CommandExtension( System.Windows.Input.ICommand command ) : base( command )
		{}

		protected CommandExtension( Binding binding )
		{
			this.binding = binding;
			Title = binding.Path.Transform( x => string.Format( "Executing '{0}'", x.Path ) );
		}

		protected override CommandModel DetermineItem( DependencyObject owner )
		{
			var result = this.MapInto<T>();
			binding.With( x => x.ApplyTo( owner, y => result.Command = result.Command ?? (System.Windows.Input.ICommand)y ) );
			return result;
		}
	}

	[ContentProperty( "Item" )]
	public class CommandModelExtension : CommandBaseExtension
	{
		public CommandModelExtension()
		{}

		public CommandModelExtension( CommandModel model )
		{
			Model = model;
		}

		public CommandModel Model { get; set; }

		protected override CommandModel DetermineItem( DependencyObject owner )
		{
			return Model;
		}
	}

	[ContentProperty( "Items" )]
	public class MonitorExtension : AttachExtension
	{
		protected override MonitorCommand CreateCommand( DependencyObject owner )
		{
			var result = base.CreateCommand( owner );
			Items.Apply( x =>
			{
				ApplyParameter( owner, x );
				result.Items.Add( x );
			} );
			Items.Clear();
			return result;
		}

		public Collection<ICommandModel> Items
		{
			get { return items; }
		}	readonly Collection<ICommandModel> items = new Collection<ICommandModel>();
	}

	public abstract class CommandBaseExtension : AttachExtension
	{
		protected CommandBaseExtension()
		{}

		protected CommandBaseExtension( System.Windows.Input.ICommand command )
		{
			Command = command;
		}

		protected override MonitorCommand CreateCommand( DependencyObject owner )
		{
			var result = base.CreateCommand( owner );
			result.Items.Add( DetermineItem( owner ).With( x => ApplyParameter( owner, x ) ) );
			return result;
		}

		protected abstract CommandModel DetermineItem( DependencyObject owner );
	}

	public abstract class AttachExtension : MarkupExtension
	{
		protected AttachExtension()
		{
			this.BuildUpOnce();
		}

		static protected void ApplyParameter( DependencyObject owner, ICommandModel item )
		{
			var property = owner.GetProperty( "CommandParameter" );
			property.With( x => owner.EnsureLoadedElement( element =>
			{
				var expression = BindingOperations.GetBindingExpression( owner, x );
				if ( expression != null )
				{
					expression.ParentBinding.ApplyTo( owner, o => item.Parameter = o );
				}
				else
				{
					item.Parameter = item.Parameter ?? owner.GetValue( x );
				}

				Dispatch.Start( item.Command.Update );

				// item.Command.Update();
			} ) );
		}

		public string Title { get; set; }

		// [DefaultPropertyValue( true )]
		public bool AllowCancel { get; set; }

		[Default( true )]
		public bool CloseOnCompletion { get; set; }

		public System.Windows.Input.ICommand Command { get; set; }

		public bool AnyExceptionIsFatal { get; set; }

		[Microsoft.Practices.Unity.Dependency]
		public ICommandRegistry Registry { get; set; }

		public override object ProvideValue( IServiceProvider serviceProvider )
		{
			var service = serviceProvider.Get<IProvideValueTarget>();
			var result = Command as MonitorCommand ?? service.TargetObject.AsTo<DependencyObject, MonitorCommand>( CreateCommand );
			service.TargetObject.As<DependencyObject>( x =>
			{
				result.Attach( x );
				x.EnsureLoadedElement( element => Registry.GetMonitor( x ).With( monitor => Registry.Register( monitor, result, x ) ) );
			} );
			return result;
		}

		protected virtual MonitorCommand CreateCommand( DependencyObject owner )
		{
			var result = new MonitorCommand ( Registry ) { Options = this.MapInto<CommandMonitorOptions>() };
			return result;
		}
	}

	public static class Commands
	{
		public static void Refresh( DependencyObject element )
		{
			Services.With<ICommandRegistry>( registry => registry.Refresh( registry.GetMonitor( element ) ) );
		}

		public static void Refresh( ICommandMonitor monitor )
		{
			Services.With<ICommandRegistry>( registry => registry.Refresh( monitor ) );
		}
	}

	[LifetimeManager( typeof(TransientLifetimeManager) )]
	public class CommandMonitor : ViewObject, ICommandMonitor
	{
		public event EventHandler Started = delegate { } , Completed = delegate { };

		readonly Queue<ICommandModel> remaining = new Queue<ICommandModel>();

		readonly ViewCollection<ICommandModel> source = new ViewCollection<ICommandModel>();
		readonly ReadOnlyObservableCollection<ICommandModel> items;

		public CommandMonitor()
		{
			items = new ReadOnlyObservableCollection<ICommandModel>( source );
		}

		void ICommandMonitor.Monitor( CommandMonitorContext context )
		{
			Monitor( context );
		}

		protected virtual void Monitor( CommandMonitorContext context )
		{
			var monitorOptions = context.Options ?? Options ?? CommandMonitorOptions.Default;
			Options = monitorOptions.Clone();

			Title = Options.Title ?? Title ?? context.Items.FirstOrDefault().Transform( x => x.Title );

			source.AddRange( context.Items.Where( x => x.IsEnabled ) );

			context.Items.Apply( x =>
			{
				x.Reset();
				remaining.Enqueue( x );
			} );

			Current.Null( () =>
			{
				Started( this, EventArgs.Empty );
				Status = ExecutionStatus.Executing;
				CommandCompleted();
			} );

			RefreshAllNotifications();
		}

		public IEnumerable<ICommandModel> Items
		{
			get { return items; }
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

		ICommandModel Current
		{
			get { return current; }
			set
			{
				current.NotNull( x => x.Executed -= CommandCompleted );
				if ( SetProperty( ref current, value, () => Current ) )
				{
					current.NotNull( x => x.Executed += CommandCompleted );
				}
			}
		}	ICommandModel current;

		public System.Windows.Input.ICommand Close
		{
			get { return closeCommand ?? ( closeCommand = new DelegateCommand( OnClose, () => AllowClose ) ); }
		}	DelegateCommand closeCommand;

		protected virtual void OnClose()
		{
			source.Apply( x => x.Reset() );

			Completed( this, EventArgs.Empty );

			source.Clear();
			remaining.Clear();

			Status = ExecutionStatus.None;
		}

		public System.Windows.Input.ICommand Cancel
		{
			get { return cancelCommand ?? ( cancelCommand = new DelegateCommand( OnCancel, () => Options.Transform( x => x.AllowCancel ) ) ); }
		}	DelegateCommand cancelCommand;

		protected virtual void OnCancel()
		{
			Options.CloseOnCompletion = false;
			Current.With( x => x.Cancel() );
		}

		public CommandMonitorOptions Options
		{
			get { return options; }
			private set
			{
				if ( SetProperty( ref options, value, () => Options ) )
				{
					UpdateCommands();
				}
			}
		}	CommandMonitorOptions options;

		
		void CommandCompleted( object sender = null, CommandCompletedEventArgs e = null )
		{
			var args = e ?? CommandCompletedEventArgs.Default;

			Options.CloseOnCompletion &= args.Error == null;

			var @continue = !args.WasCancelled && ( args.Error == null || ( !Options.AnyExceptionIsFatal && Current.Transform( x => x.ExceptionHandlingAction == CommandExceptionHandlingAction.Continue, () => true ) ) );
			if ( @continue )
			{
				PercentageComplete = ( source.IndexOf( Current ) + 2d ) / ( source.Count + 1d );

				Current = remaining.Any() ? remaining.Dequeue() : null;
				if ( Current != null )
				{
					try
					{
						Current.Execute();
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
			var queue = Current.Transform( x => source.Skip( source.IndexOf( x ) + 1 ) );
			queue.Apply( x => x.Cancel() );

			PercentageComplete = 1;
			Status = wasCanceled ? ExecutionStatus.Canceled : source.FirstOrDefault( x => x.Exception != null ).Transform( x => Options.AnyExceptionIsFatal || x.Exception is FatalApplicationException ? ExecutionStatus.CompletedWithFatalException : ExecutionStatus.CompletedWithException, () => ExecutionStatus.Completed );

			AllowClose = Status != ExecutionStatus.CompletedWithFatalException;
			Options.AllowCancel = false;

			var close = AllowClose && Options.CloseOnCompletion && exception == null;
			close.IsTrue( OnClose );

			var propagate = Current.Transform( x => x.Exception != null && x.ExceptionHandlingAction == CommandExceptionHandlingAction.Throw ? x.Exception : null );

			Current = null;

			if ( propagate != null )
			{
				throw propagate;
			}
		}
	}

	[LifetimeManager( typeof(ContainerControlledLifetimeManager) )]
	public class ApplicationCommandMonitor : CommandMonitor
	{
		readonly IEventAggregator aggregator;

		readonly ConditionMonitor initialized = new ConditionMonitor(), loaded = new ConditionMonitor(), completed = new ConditionMonitor();

		public ApplicationCommandMonitor( IEventAggregator aggregator )
		{
			this.aggregator = aggregator;
		}

		protected override void Monitor( CommandMonitorContext context )
		{
			initialized.Apply( () =>
			{
				var done = !context.Items.Any();
				var items = SetupStatus.Loading.Append( done ? new[] { SetupStatus.Loaded, SetupStatus.Complete } : Enumerable.Empty<SetupStatus>() );
				items.Apply( Publish );
			} );

			base.Monitor( context );
		}

		[Default( "The application is currently busy." )]
		public override string Title
		{
			get { return base.Title; }
			set { base.Title = value; }
		}

		protected override void OnCompleted( System.Exception exception = null, bool wasCanceled = false )
		{
			loaded.Apply( () => Publish( SetupStatus.Loaded ) );
			
			base.OnCompleted( exception, wasCanceled );
		}

		protected override void OnClose()
		{
			base.OnClose();

			completed.Apply( () => Publish( SetupStatus.Complete ) );
		}

		void Publish( SetupStatus status )
		{
			Dispatch.Start( () => 
				aggregator.GetEvent<SetupEvent>().With( x => Dispatch.Execute( () => x.Publish( status ) ) ) 
			);
		}
	}


	[ContentProperty( "Command" )]
	public class RefreshCommandingAction : TargetedTriggerAction<DependencyObject>
	{
		protected override void Invoke( object parameter )
		{
			Commands.Refresh( Target );
		}
	}

	[ContentProperty( "Command" )]
	public sealed class InvokeCommandAction : TriggerAction<DependencyObject>, Presentation.IAttachedObject
	{
		public event EventHandler Attached = delegate { } , Detached = delegate { };

		string commandName;
		public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register( "CommandParameter", typeof(object), typeof(InvokeCommandAction), null );
		public static readonly DependencyProperty CommandProperty = DependencyProperty.Register( "Command", typeof(System.Windows.Input.ICommand), typeof(InvokeCommandAction), null );

		protected override void OnAttached()
		{
			base.OnAttached();
			Attached( this, EventArgs.Empty );
		}

		protected override void OnDetaching()
		{
			base.OnDetaching();
			Detached( this, EventArgs.Empty );
		}

		protected override void Invoke( object parameter )
		{
			if ( AssociatedObject != null )
			{
				var command = ResolveCommand();
				if ( ( command != null ) && command.CanExecute( CommandParameter ) )
				{
					command.Execute( CommandParameter );
				}
			}
		}

		System.Windows.Input.ICommand ResolveCommand()
		{
			System.Windows.Input.ICommand command = null;
			if ( Command == null )
			{
				if ( AssociatedObject != null )
				{
					foreach ( var info in AssociatedObject.GetType().GetProperties( ( BindingFlags.Public ) | ( BindingFlags.Instance ) ).Where( info => typeof(System.Windows.Input.ICommand).IsAssignableFrom( info.PropertyType ) && string.Equals( info.Name, CommandName, StringComparison.Ordinal ) ) )
					{
						command = (System.Windows.Input.ICommand)info.GetValue( AssociatedObject, null );
					}
				}
				return command;
			}
			return Command;
		}

		public System.Windows.Input.ICommand Command
		{
			get { return (System.Windows.Input.ICommand)GetValue( CommandProperty ); }
			set { SetValue( CommandProperty, value ); }
		}

		public string CommandName
		{
			get { return commandName; }
			set
			{
				if ( CommandName != value )
				{
					commandName = value;
				}
			}
		}

		public object CommandParameter
		{
			get { return GetValue( CommandParameterProperty ); }
			set { SetValue( CommandParameterProperty, value ); }
		}
	}
	
	public class ExceptionHandlingCommand : CommandBase<Exception>
	{
		readonly IExceptionHandler handler;

		public ExceptionHandlingCommand( IExceptionHandler handler )
		{
			this.handler = handler;
		}

		protected override void Execute( Exception parameter )
		{
			handler.Handle( parameter );
		}
	}
}