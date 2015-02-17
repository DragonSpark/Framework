using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using DragonSpark.Extensions;
using Xamarin.Forms;
using Binding = System.Windows.Data.Binding;
using Point = System.Windows.Point;

namespace DragonSpark.Client.Windows.Forms.Rendering
{
	static class FrameworkElementExtensions
	{
		public static IEnumerable<T> FindElementsAt<T>( this FrameworkElement @this, Point point ) where T : UIElement
		{
			var result = Find<T>( @this, new PointHitTestParameters( point ) );
			return result;
		}
		
		public static IEnumerable<T> FindElementsAt<T>( this FrameworkElement @this, Rect bounds ) where T : UIElement
		{
			var result = Find<T>( @this, new GeometryHitTestParameters( new RectangleGeometry( bounds ) ) );
			return result;
		}

		static IEnumerable<T> Find<T>( Visual @this, HitTestParameters parameters ) where T : UIElement
		{
			var result = new List<T>();
			VisualTreeHelper.HitTest( @this, null, hit =>
			{
				hit.VisualHit.As<T>( result.Add );
				return HitTestResultBehavior.Continue;
			},
				parameters );
			return result.AsReadOnly();
		}

		static readonly Lazy<ConcurrentDictionary<Type, DependencyProperty>> ForegroundProperties = new Lazy<ConcurrentDictionary<Type, DependencyProperty>>( () => new ConcurrentDictionary<Type, DependencyProperty>() );

		public static Brush GetForeground( this FrameworkElement element )
		{
			if ( element == null )
			{
				throw new ArgumentNullException( "element" );
			}
			return (Brush)element.GetValue( GetForegroundProperty( element ) );
		}

		public static Binding GetForegroundBinding( this FrameworkElement element )
		{
			var bindingExpression = element.GetBindingExpression( GetForegroundProperty( element ) );
			if ( bindingExpression == null )
			{
				return null;
			}
			return bindingExpression.ParentBinding;
		}

		public static void SetForeground( this FrameworkElement element, Brush foregroundBrush )
		{
			if ( element == null )
			{
				throw new ArgumentNullException( "element" );
			}
			element.SetValue( GetForegroundProperty( element ), foregroundBrush );
		}

		public static void SetForeground( this FrameworkElement element, Binding binding )
		{
			if ( element == null )
			{
				throw new ArgumentNullException( "element" );
			}
			element.SetBinding( GetForegroundProperty( element ), binding );
		}

		static DependencyProperty GetForegroundProperty( FrameworkElement element )
		{
			if ( element is Control )
			{
				return Control.ForegroundProperty;
			}
			if ( element is TextBlock )
			{
				return TextBlock.ForegroundProperty;
			}
			var type = element.GetType();
			DependencyProperty result;
			if ( ForegroundProperties.Value.TryGetValue( type, out result ) )
			{
				return result;
			}
			var fieldInfo = type.GetFields( BindingFlags.Public | BindingFlags.Static ).FirstOrDefault( ( FieldInfo f ) => f.Name == "ForegroundProperty" );
			if ( fieldInfo == null )
			{
				throw new ArgumentException( "type is not a Foregroundable type" );
			}
			var dependencyProperty = (DependencyProperty)fieldInfo.GetValue( null );
			ForegroundProperties.Value.TryAdd( type, dependencyProperty );
			return dependencyProperty;
		}
	}
}
