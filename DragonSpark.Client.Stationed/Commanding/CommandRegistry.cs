using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows;
using DragonSpark.Client.Stationed.Extensions;
using DragonSpark.Extensions;
using Xceed.Wpf.Toolkit;

namespace DragonSpark.Client.Stationed.Commanding
{
	public class CommandRegistry : ICommandRegistry
	{
		readonly ConditionalWeakTable<ICommandMonitor, List<DependencyObject>> items = new ConditionalWeakTable<ICommandMonitor, List<DependencyObject>>();

		readonly ConditionalWeakTable<FrameworkElement, List<DependencyObject>> references = new ConditionalWeakTable<FrameworkElement, List<DependencyObject>>();

		public void Register( ICommandMonitor monitor, ICommand command, DependencyObject context )
		{
			var list = items.GetOrCreateValue( monitor );
			if ( !list.Contains( context ) )
			{
				var commandParameterProperty = context.GetProperty( "CommandParameter" );
				context.SetValue( commandParameterProperty, monitor );
				
				list.Add( context );

				var element = context.DetermineFrameworkElement();
				SetMonitor( element, monitor );

				references.GetOrCreateValue( element ).Add( context );

				element.Unloaded += CommandRegistry_Unloaded;
			}
		}

		void CommandRegistry_Unloaded( object sender, RoutedEventArgs e )
		{
			var element = sender.To<FrameworkElement>();
			element.Unloaded -= CommandRegistry_Unloaded;

			var monitor = GetMonitor( element );

			var objects = references.GetOrCreateValue( element );
			objects.Apply( x =>
			{
				items.GetOrCreateValue( monitor ).Remove( x );
			} );
			objects.Clear();
		}

		public void Refresh( ICommandMonitor monitor )
		{
			items.GetOrCreateValue( monitor ).Apply( x => x.GetProperty( "Command" ).With( y => x.GetValue( y ).As<System.Windows.Input.ICommand>( z => z.Update() ) ) );
		}

		public ICommandMonitor GetMonitor( DependencyObject element )
		{
			var result = element.GetValueByHierarchy<ICommandMonitor>( BusyIndicator.BusyContentProperty );
			return result;
		}

		public static readonly DependencyProperty MonitorProperty = DependencyProperty.RegisterAttached( "Commands", typeof(ICommandMonitor), typeof(CommandRegistry), new PropertyMetadata( OnMonitorPropertyChanged ) );

		static void OnMonitorPropertyChanged( DependencyObject o, DependencyPropertyChangedEventArgs e )
		{}

		public static ICommandMonitor GetMonitor( FrameworkElement element )
		{
			return (ICommandMonitor)element.GetValue( MonitorProperty );
		}

		public static void SetMonitor( FrameworkElement element, ICommandMonitor value )
		{
			element.SetValue( MonitorProperty, value );
		}
	}
}