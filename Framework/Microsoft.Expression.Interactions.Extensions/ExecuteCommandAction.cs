using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;

namespace Microsoft.Expression.Interactions.Extensions
{
	public class ExecuteRoutedUiCommandAction : ExecuteCommandActionBase<string>
	{
		readonly static CommandConverter TypeConverter = new CommandConverter();
		protected override void Invoke(object parameter)
		{
			var command = Convert();
			command.Execute( CommandParameter, Target );
		}

		RoutedUICommand Convert()
		{
			try
			{
				return TypeConverter.ConvertFromString( Command ) as RoutedUICommand;
			}
			catch ( Exception e )
			{
				throw new ArgumentException( String.Format( "Unable to convert \"{0}\" to a routed event. Verify that the specified string is a properly registered routed event and try again.", this.Command ), e );
			}
		}
	}

	public class ExecuteCommandAction : ExecuteCommandActionBase<ICommand>
	{
		protected override void Invoke( object parameter )
		{
			Command.Execute( CommandParameter );
		}
	}

	/// <summary>
	/// Executes a routed command
	/// </summary>
	[ContentProperty( "Command" )]
	public abstract class ExecuteCommandActionBase<TCommand> : System.Windows.Interactivity.TargetedTriggerAction<UIElement>
	{
		/// <summary>
		/// Name of the command to be run
		/// </summary>
		public TCommand Command
		{
			get { return (TCommand)GetValue(CommandProperty); }
			set { SetValue(CommandProperty, value); }
		}	public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(TCommand), typeof(ExecuteCommandActionBase<TCommand>), new UIPropertyMetadata(null));

		/// <summary>
		/// Optional parameter for command.
		/// </summary>
		public object CommandParameter
		{
			get { return (object)GetValue(CommandParameterProperty); }
			set { SetValue(CommandParameterProperty, value); }
		}	public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register("CommandParameter", typeof(object), typeof(ExecuteCommandActionBase<TCommand>), new UIPropertyMetadata(null));
	}
}