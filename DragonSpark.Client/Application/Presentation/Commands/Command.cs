using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interactivity;
using DragonSpark.Application.Presentation.Controls;
using DragonSpark.Application.Presentation.Extensions;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.Commands
{
	public static class Command
	{
		readonly static IList<WeakReference> Items = new List<WeakReference>();

		public static readonly DependencyProperty AttachProperty = DependencyProperty.RegisterAttached( "Attach", typeof(ICommand), typeof(Command), new PropertyMetadata( OnAssignPropertyChanged ) );

		static void OnAssignPropertyChanged( DependencyObject o, DependencyPropertyChangedEventArgs e )
		{
			o.EnsureLoadedElement( x => e.NewValue.As<ICommand>( y =>
			{
				var commandProperty = o.GetProperty( "Command" );
				var commandParameterProperty = o.GetProperty( "CommandParameter" );

				var indicator = x.GetVisualAncestorsAndSelf().FirstOrDefaultOfType<BusyIndicator>();
				o.GetValue( commandParameterProperty ).Null( () => o.SetValue( commandParameterProperty, indicator.BusyContent ) );
				
				var command = y.As<ExecuteCommandSetCommand>() ?? new ExecuteCommandSetCommand().With( a => a.Commands.Add( y.To<IMonitoredCommand>() ) );
				command.As<IAttachedObject>( z => z.Attach( x ) );

				o.SetValue( commandProperty, command );

				Items.Add( new WeakReference( x ) );
			} ) );
		}

		public static ExecuteCommandSetCommand GetAttach( DependencyObject element )
		{
			return (ExecuteCommandSetCommand)element.GetValue( AttachProperty );
		}

		public static void SetAttach( DependencyObject element, ICommand value )
		{
			element.SetValue( AttachProperty, value );
		}

		public static void Refresh()
		{
			Items.Targets<FrameworkElement>().Apply( x => x.GetProperty( "Command" ).NotNull( x.RefreshValue ) );
		}
	}
}