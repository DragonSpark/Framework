using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;
using IAttachedObject = DragonSpark.Application.Presentation.ComponentModel.IAttachedObject;

namespace DragonSpark.Application.Presentation.Interaction
{
	public sealed class InvokeCommandAction : TriggerAction<DependencyObject>, IAttachedObject
	{
		public event EventHandler Attached = delegate { } , Detached = delegate { };

		string commandName;
		public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register( "CommandParameter", typeof(object), typeof(InvokeCommandAction), null );
		public static readonly DependencyProperty CommandProperty = DependencyProperty.Register( "Command", typeof(ICommand), typeof(InvokeCommandAction), null );

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

		ICommand ResolveCommand()
		{
			ICommand command = null;
			if ( Command == null )
			{
				if ( AssociatedObject != null )
				{
					foreach ( var info in AssociatedObject.GetType().GetProperties( ( BindingFlags.Public ) | ( BindingFlags.Instance ) ).Where( info => typeof(ICommand).IsAssignableFrom( info.PropertyType ) && string.Equals( info.Name, CommandName, StringComparison.Ordinal ) ) )
					{
						command = (ICommand)info.GetValue( base.AssociatedObject, null );
					}
				}
				return command;
			}
			return Command;
		}

		public ICommand Command
		{
			get { return (ICommand)GetValue( CommandProperty ); }
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
}