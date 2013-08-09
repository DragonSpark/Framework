using System.Windows;
using System.Windows.Controls;
using DragonSpark.Application.Presentation.Extensions;
using DragonSpark.Extensions;
using Microsoft.Expression.Interactivity.Core;

namespace DragonSpark.Application.Presentation
{
	public static class VisualState
	{
		public static readonly DependencyProperty CurrentStateProperty =
			DependencyProperty.RegisterAttached( "CurrentState", typeof(string), typeof(VisualState),
			                                     new PropertyMetadata( OnCurrentStatePropertyChanged ) );

		public static readonly DependencyProperty UseTransitionsProperty =
			DependencyProperty.RegisterAttached( "UseTransitions", typeof(bool), typeof(VisualState),
			                                     new PropertyMetadata( true, OnUseTransitionsPropertyChanged ) );

		static void OnUseTransitionsPropertyChanged( DependencyObject o, DependencyPropertyChangedEventArgs e )
		{}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Constraint for Xaml property." )]
		public static bool GetUseTransitions( FrameworkElement element )
		{
			return (bool)element.GetValue( UseTransitionsProperty );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Constraint for Xaml property." )]
		public static void SetUseTransitions( FrameworkElement element, bool value )
		{
			element.SetValue( UseTransitionsProperty, value );
		}

		static void OnCurrentStatePropertyChanged( DependencyObject o, DependencyPropertyChangedEventArgs e )
		{
			o.To<FrameworkElement>().EnsureLoaded( x => e.NewValue.To<string>().NotNull( y =>
			{
			    var control = x.GetParentOfType<Control>( true );
			    VisualStateManager.GoToState( control, y, GetUseTransitions( x ) );
			} ) );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Constraint for Xaml property." )]
		public static string GetCurrentState( Control element )
		{
			return (string)element.GetValue( CurrentStateProperty );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Constraint for Xaml property." )]
		public static void SetCurrentState( Control element, string value )
		{
			element.SetValue( CurrentStateProperty, value );
		}

		public static readonly DependencyProperty CurrentElementStateProperty = DependencyProperty.RegisterAttached( "CurrentElementState", typeof(string), typeof(VisualState),
			                                     new PropertyMetadata( OnCurrentElementStatePropertyChanged ) );

		static void OnCurrentElementStatePropertyChanged( DependencyObject o, DependencyPropertyChangedEventArgs e )
		{
			o.To<FrameworkElement>().EnsureLoaded( x => e.NewValue.To<string>().NotNull( y => ExtendedVisualStateManager.GoToElementState( x, y, GetUseTransitions( x ) ) ) );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Type is used on property change." )]
		public static string GetCurrentElementState( FrameworkElement element )
		{
			return (string)element.GetValue( CurrentElementStateProperty );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Type is used on property change." )]
		public static void SetCurrentElementState( FrameworkElement element, string value )
		{
			element.SetValue( CurrentElementStateProperty, value );
		}
	}
}