using System.Windows;
using DragonSpark.Application.Presentation.Extensions;
using DragonSpark.Extensions;
using DragonSpark.Runtime;

namespace DragonSpark.Application.Presentation.Design
{
	public static class DesignTimeCompensations
	{
		public static readonly DependencyProperty SettersProperty =
			DependencyProperty.RegisterAttached( "Setters", typeof(DesignTimePropertySetterCollection), typeof(DesignTimeCompensations),
			                                     new PropertyMetadata( OnBindingsPropertyChanged ) );

		static void OnBindingsPropertyChanged( DependencyObject o, DependencyPropertyChangedEventArgs e )
		{
			Environment.IsInDesignMode.IsTrue( () => o.To<FrameworkElement>().EnsureLoaded( x => e.NewValue.To<DesignTimePropertySetterCollection>().Apply( y =>
			{
				x.ClearValue( y.Property );
				var value = y.Property.GetMetadata( x.GetType() ).DefaultValue.Transform( z => y.Value.ConvertTo( z.GetType() ), () => y.Value );
				x.SetValue( y.Property, value );
			} ) ) );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
		public static DesignTimePropertySetterCollection GetSetters( FrameworkElement element )
		{
			return (DesignTimePropertySetterCollection)element.GetValue( SettersProperty );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
		public static void SetSetters( FrameworkElement element, DesignTimePropertySetterCollection value )
		{
			element.SetValue( SettersProperty, value );
		}

		/*public static readonly DependencyProperty ScreenCollectionProperty =
			DependencyProperty.RegisterAttached( "ScreenCollection", typeof(ScreenCollection), typeof(DesignTimeCompensations),
			                                     new PropertyMetadata( OnScreenCollectionPropertyChanged ) );

		static void OnScreenCollectionPropertyChanged( DependencyObject o, DependencyPropertyChangedEventArgs e )
		{}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
		public static ScreenCollection GetScreenCollection( FrameworkElement element )
		{
			return (ScreenCollection)element.GetValue( ScreenCollectionProperty );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
		public static void SetScreenCollection( FrameworkElement element, ScreenCollection value )
		{
			element.SetValue( ScreenCollectionProperty, value );
		}*/
	}
}