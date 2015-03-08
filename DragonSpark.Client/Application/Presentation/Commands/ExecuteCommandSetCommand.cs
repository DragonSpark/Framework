using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;
using DragonSpark.Application.Presentation.ComponentModel;
using DragonSpark.Application.Presentation.Extensions;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.Commands
{
	[ContentProperty( "Commands" )]
	public class ExecuteCommandSetCommand : CommandBase<ICommandMonitor>, IAttachedObject
	{
		public event EventHandler Completed = delegate { };

		readonly ViewAwareSupporter viewAwareSupporter;
		
		public ExecuteCommandSetCommand()
		{
			viewAwareSupporter = new ViewAwareSupporter( this );
		}

		public CommandMonitorOptions Options
		{
			get { return options; }
			set { SetProperty( ref options, value, () => Options ); }
		}	CommandMonitorOptions options = CommandMonitorOptions.Default;

		public ObservableCollection<IMonitoredCommand> Commands
		{
			get { return commands; }
		}	readonly ObservableCollection<IMonitoredCommand> commands = new ObservableCollection<IMonitoredCommand>();

		protected override bool CanExecute( ICommandMonitor parameter )
		{
			var result = base.CanExecute( parameter ) && Commands.All( x => x.CanExecute( parameter ) );
			return result;
		}

		protected override void Execute( ICommandMonitor monitor )
		{
			monitor.Completed += MonitorOnCompleted;
			var context = new CommandMonitorContext( Commands, Options );
			monitor.Monitor( context );
		}

		void MonitorOnCompleted( object sender, EventArgs eventArgs )
		{
			sender.As<ICommandMonitor>( x =>
			{
				x.Completed += MonitorOnCompleted;
				Completed( this, EventArgs.Empty );

				Command.Refresh();
			} );
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

		void System.Windows.Interactivity.IAttachedObject.Attach( DependencyObject dependencyObject )
		{
			viewAwareSupporter.Attach( dependencyObject );
			OnAttached();
		}

		protected virtual void OnAttached()
		{
			Commands.OfType<IAttachedObject>().Apply( x => x.Attach( AssociatedObject ) );
		}

		void System.Windows.Interactivity.IAttachedObject.Detach()
		{
			OnDetached();
		}

		protected virtual void OnDetached()
		{
			Commands.OfType<IAttachedObject>().Apply( x => x.Detach() );
		}

		public DependencyObject AssociatedObject
		{
			get { return viewAwareSupporter.AssociatedObject; }
		}
		#endregion
	}
}