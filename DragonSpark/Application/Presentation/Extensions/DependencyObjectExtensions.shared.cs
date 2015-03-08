using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using DragonSpark.Extensions;
using Microsoft.Practices.ServiceLocation;

namespace DragonSpark.Application.Presentation.Extensions
{
	public static partial class DependencyObjectExtensions
	{
		/*public static IInteractionNode ResolveHandlingNode(this DependencyObject target)
		{
			Contract.Requires( target != null );

			var result = target.GetValueByHierarchy<InteractionNode>( DefaultRoutedMessageController.NodeProperty, item => item.MessageHandler != null );
			return result;
		}*/

		public static TResult EnsureWithNewInstance<TResult>(this DependencyObject target, DependencyProperty property, Action<DependencyObject, TResult> setter) where TResult : new()
		{
			return Ensure(target, property, setter, () => new TResult());
		}

		public static TResult Ensure<TResult>(this DependencyObject target, DependencyProperty property, Action<DependencyObject, TResult> setter)
		{
			return Ensure(target, property, setter, () => ServiceLocator.Current.GetInstance<TResult>());
		}

		public static TResult Ensure<TResult>(this DependencyObject target, DependencyProperty property, Action<DependencyObject, TResult> setter, Func<TResult> creator)
		{
			var result = target.GetValue(property).To<TResult>();
			if (Equals(result, default(TResult)))
			{
				setter(target, result = creator());
			}
			return result;
		}

		public static IEnumerable<DependencyObject> GetChildren( this DependencyObject target )
		{
			var count = VisualTreeHelper.GetChildrenCount( target );
			for ( var i = 0; i < count; i++ )
			{
				yield return VisualTreeHelper.GetChild( target, i );
			}
		}

		public static TElement GetParentElementContaining<TElement>( this DependencyObject target, DependencyProperty property, bool includeSelf = true ) where TElement : FrameworkElement
		{
			for ( var current = includeSelf ? target : VisualTreeHelper.GetParent( target ); current != null; current = VisualTreeHelper.GetParent( current ) )
			{
				var value = current.ReadLocalValue( property );
				if ( value != DependencyProperty.UnsetValue )
				{
					var result = current.As<TElement>();
					return result;
				}
			}
			return null;
		}

		public static TPropertyType GetValueByHierarchy<TPropertyType>( this DependencyObject target, DependencyProperty property, Func<TPropertyType,bool> where = null, bool includeSelf = true )
		{
			for ( var current = includeSelf ? target : VisualTreeHelper.GetParent( target ); current != null; current = VisualTreeHelper.GetParent( current ) )
			{
				var value = current.ReadLocalValue( property );
				if ( value != DependencyProperty.UnsetValue )
				{
					var source = value.As<BindingExpression>().Transform( x =>
					                                                      	{
																				x.UpdateSource();
																				return x.ParentBinding.Source;
					                                                      	} ) ?? value;

					var result = source.To<TPropertyType>();
					if ( where == null || where( result ) )
					{
						return result;
					}
				}
			}
			return default(TPropertyType);
		}

		public static TParentType GetParentOfType<TParentType>( this DependencyObject target ) where TParentType : class
		{
			var result = GetParentOfType<TParentType>( target, false );
			return result;
		}

		public static TParentType GetParentOfType<TParentType>( this DependencyObject target, bool includeSelf ) where TParentType : class 
		{
			for ( var current = includeSelf ? target : VisualTreeHelper.GetParent( target );
			      current != null; current = VisualTreeHelper.GetParent( current ) )
			{
				var parent = current.As<TParentType>();
				if ( parent != null )
				{
					return parent;
				}
			}
			return null;
		}

		public static TResultType GetChild<TResultType>( this DependencyObject target ) where TResultType : DependencyObject
		{
			var children = target.GetChildren();
			var result = children.OfType<TResultType>().FirstOrDefault();
			return result;
		}

		public static TResultType FindName<TResultType>( this DependencyObject target, string name ) where TResultType : DependencyObject
		{
			var element = target.As<FrameworkElement>();
			var result = ( element != null ? (TResultType)element.FindName( name ) : null ) ?? FindNameInternal<TResultType>( target, name );
			return result;
		}

		public static DependencyProperty GetProperty( this DependencyObject target, string name )
		{
			var type = target.GetType();
			var candidates = new[] { name, string.Format( "{0}Property", name ) };
			var result = candidates.Select( x => type.GetField( x, DragonSparkBindingOptions.AllProperties ) ).FirstOrDefault( x => x != null ).Transform( x => x.GetValue( null ).As<DependencyProperty>() );
			return result;
		}

		static TResultType FindNameInternal<TResultType>( this DependencyObject target, string name ) where TResultType : DependencyObject
		{
			var list = new List<TResultType>();
			Func<DependencyObject, TResultType> resolver = item => item.As<TResultType>() != null && string.Compare( (string)item.GetValue( FrameworkElement.NameProperty ), name ) == 0 ? item.As<TResultType>() : null;
			SearchFor( list, resolver, target, 1 );
			var result = list.FirstOrDefault();
			return result;
		}

		internal static void SearchFor<TControlType>( ICollection<TControlType> collection, Func<DependencyObject, TControlType> resolver, DependencyObject target ) where TControlType : class
		{
			SearchFor( collection, resolver, target, null );
		}

		internal static bool SearchFor<TControlType>( ICollection<TControlType> collection, Func<DependencyObject, TControlType> resolver, DependencyObject target, int? maxCount ) where TControlType : class
		{
			Contract.Assume( collection != null && resolver != null && target != null );

			var item = resolver( target );
			item.NotNull( collection.Add );

			var result = maxCount == null || collection.Count < maxCount;
			result.IsTrue( () =>
			{
				foreach ( var child in ResolveChildren( target ) )
				{
					if ( !SearchFor( collection, resolver, child, maxCount ) )
					{
						break;
					}
				}
			} );
			return result;
		}

		static IEnumerable<DependencyObject> ResolveChildren( DependencyObject target )
		{
			var result = target is UIElement ? target.GetChildren().ToList() : ResolveNonVisualChildren( target );
			return result;
		}

		static IEnumerable<DependencyObject> ResolveNonVisualChildren( DependencyObject target )
		{
			var property = ResolveChildrenProperty( target );
			var result = property != null ? property.GetValue( target, null ).To<IEnumerable>().Cast<DependencyObject>() : new List<DependencyObject>();
			return result;
		}

		static PropertyInfo ResolveChildrenProperty( DependencyObject target )
		{
			var type = target.GetType();
			var list = new List<string>( new[] { "Children" } );
			var content = type.GetAttribute<ContentPropertyAttribute>();
			if ( content != null )
			{
				list.Insert( 0, content.Name );
			}

			var result = ( from name in list
			               let property = type.GetProperty( name )
			               where property != null && typeof(IEnumerable).IsAssignableFrom( property.PropertyType )
			               select property ).FirstOrDefault();
			return result;
		}
	}
}