using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Interactivity;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using DragonSpark.Application.Client.Presentation;
using DragonSpark.Application.Runtime;
using DragonSpark.Extensions;
using Microsoft.Expression.Interactions.Extensions.DataHelpers;
using Microsoft.Practices.ServiceLocation;
using Xceed.Wpf.Toolkit;
using IAttachedObject = System.Windows.Interactivity.IAttachedObject;

// using IAttachedObject = DragonSpark.Client.Windows.Presentation.IAttachedObject;

namespace DragonSpark.Application.Client.Extensions
{
	public static class BindingExtensions
	{
		static readonly IList<WeakReference<Binding>> References = new List<WeakReference<Binding>>();

		public static void ApplyTo( this Binding @this, DependencyObject source, Action<object> setter )
		{
			References.CheckWith( @this, x =>
			{
				new BindingListener( ( s, a ) => setter( a.EventArgs.NewValue.Transform( item => Equals( @this.Source, source ) && Equals( item, source ) ? item.AsTo<FrameworkElement, object>( e => e.DataContext ) : item ) ) ) { Binding = @this, Element = source };
			} );
		}
	}

	public static class ExtensionMethods
	{
		/// <summary>
		/// Converts an expression into a <see cref="MemberInfo" />.
		/// </summary>
		/// <param name="expression">The expression to convert.</param>
		/// <returns>The member info.</returns>
		public static MemberInfo GetMemberInfo(this System.Linq.Expressions.Expression expression) {
			var lambda = (LambdaExpression)expression;

			MemberExpression memberExpression;
			if(lambda.Body is UnaryExpression) {
				var unaryExpression = (UnaryExpression)lambda.Body;
				memberExpression = (MemberExpression)unaryExpression.Operand;
			}
			else
				memberExpression = (MemberExpression)lambda.Body;

			return memberExpression.Member;
		}
	}

	public static class VisualTreeExtensions
	{
		public static T GetVisualAncestor<T>( this DependencyObject element )
			where T : DependencyObject
		{
			return GetVisualAncestorsAndSelfIterator( element )
				.Skip( 1 )
				.OfType<T>()
				.FirstOrDefault();
		}

		public static DependencyObject GetVisualAncestor( this DependencyObject element, Type type )
		{
			return GetVisualAncestorsAndSelfIterator( element )
				.Skip( 1 )
				.FirstOrDefault( d => d.GetType() == type );
		}

		/// <summary>
		/// Get the visual tree ancestors of an element.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <returns>The visual tree ancestors of the element.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="element"/> is null.
		/// </exception>
		public static IEnumerable<DependencyObject> GetVisualAncestors( this DependencyObject element )
		{
			if ( element == null )
				throw new ArgumentNullException( "element" );

			return GetVisualAncestorsAndSelfIterator( element ).Skip( 1 );
		}

		/// <summary>
		/// Get the visual tree ancestors of an element and the element itself.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <returns>
		/// The visual tree ancestors of an element and the element itself.
		/// </returns>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="element"/> is null.
		/// </exception>
		public static IEnumerable<DependencyObject> GetVisualAncestorsAndSelf( this DependencyObject element )
		{
			if ( element == null )
				throw new ArgumentNullException( "element" );

			return GetVisualAncestorsAndSelfIterator( element );
		}

		/// <summary>
		/// Get the visual tree ancestors of an element and the element itself.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <returns>
		/// The visual tree ancestors of an element and the element itself.
		/// </returns>
		static IEnumerable<DependencyObject> GetVisualAncestorsAndSelfIterator( DependencyObject element )
		{
			Debug.Assert( element != null, "element should not be null!" );

			for ( DependencyObject o = element;
				o != null;
				o = VisualTreeHelper.GetParent( o ) )
				yield return o;
		}

		/// <summary>
		/// Get the visual tree children of an element.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <returns>The visual tree children of an element.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="element"/> is null.
		/// </exception>
		public static IEnumerable<DependencyObject> GetVisualChildren( this DependencyObject element )
		{
			if ( element == null )
				throw new ArgumentNullException( "element" );

			return GetVisualChildrenAndSelfIterator( element ).Skip( 1 );
		}

		/// <summary>
		/// Get the visual tree children of an element and the element itself.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <returns>
		/// The visual tree children of an element and the element itself.
		/// </returns>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="element"/> is null.
		/// </exception>
		public static IEnumerable<DependencyObject> GetVisualChildrenAndSelf( this DependencyObject element )
		{
			if ( element == null )
				throw new ArgumentNullException( "element" );

			return GetVisualChildrenAndSelfIterator( element );
		}

		/// <summary>
		/// Get the visual tree children of an element and the element itself.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <returns>
		/// The visual tree children of an element and the element itself.
		/// </returns>
		static IEnumerable<DependencyObject> GetVisualChildrenAndSelfIterator( this DependencyObject element )
		{
			Debug.Assert( element != null, "element should not be null!" );

			yield return element;

			int count = VisualTreeHelper.GetChildrenCount( element );
			for ( int i = 0; i < count; i++ )
				yield return VisualTreeHelper.GetChild( element, i );
		}

		public static T GetVisualDescendant<T>( this DependencyObject element )
			where T : DependencyObject
		{
			if ( element == null )
				throw new ArgumentNullException( "element" );

			return GetVisualDescendantsAndSelfIterator( element )
				.Skip( 1 )
				.OfType<T>()
				.FirstOrDefault();
		}

		/// <summary>
		/// Get the visual tree descendants of an element.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <returns>The visual tree descendants of an element.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="element"/> is null.
		/// </exception>
		public static IEnumerable<DependencyObject> GetVisualDescendants( this DependencyObject element )
		{
			if ( element == null )
				throw new ArgumentNullException( "element" );

			return GetVisualDescendantsAndSelfIterator( element ).Skip( 1 );
		}

		/// <summary>
		/// Get the visual tree descendants of an element and the element
		/// itself.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <returns>
		/// The visual tree descendants of an element and the element itself.
		/// </returns>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="element"/> is null.
		/// </exception>
		public static IEnumerable<DependencyObject> GetVisualDescendantsAndSelf( this DependencyObject element )
		{
			if ( element == null )
				throw new ArgumentNullException( "element" );

			return GetVisualDescendantsAndSelfIterator( element );
		}

		/// <summary>
		/// Get the visual tree descendants of an element and the element
		/// itself.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <returns>
		/// The visual tree descendants of an element and the element itself.
		/// </returns>
		static IEnumerable<DependencyObject> GetVisualDescendantsAndSelfIterator( DependencyObject element )
		{
			Debug.Assert( element != null, "element should not be null!" );

			var remaining = new Queue<DependencyObject>();
			remaining.Enqueue( element );

			while ( remaining.Count > 0 )
			{
				DependencyObject obj = remaining.Dequeue();
				yield return obj;

				foreach ( DependencyObject child in obj.GetVisualChildren() )
					remaining.Enqueue( child );
			}
		}

		/// <summary>
		/// Get the visual tree siblings of an element.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <returns>The visual tree siblings of an element.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="element"/> is null.
		/// </exception>
		public static IEnumerable<DependencyObject> GetVisualSiblings( this DependencyObject element )
		{
			return element
				.GetVisualSiblingsAndSelf()
				.Where( p => p != element );
		}

		/// <summary>
		/// Get the visual tree siblings of an element and the element itself.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <returns>
		/// The visual tree siblings of an element and the element itself.
		/// </returns>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="element"/> is null.
		/// </exception>
		public static IEnumerable<DependencyObject> GetVisualSiblingsAndSelf( this DependencyObject element )
		{
			if ( element == null )
				throw new ArgumentNullException( "element" );

			DependencyObject parent = VisualTreeHelper.GetParent( element );
			return parent == null
				? Enumerable.Empty<DependencyObject>()
				: parent.GetVisualChildren();
		}

		/// <summary>
		/// Get the bounds of an element relative to another element.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <param name="otherElement">
		/// The element relative to the other element.
		/// </param>
		/// <returns>
		/// The bounds of the element relative to another element, or null if
		/// the elements are not related.
		/// </returns>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="element"/> is null.
		/// </exception>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="otherElement"/> is null.
		/// </exception>
		public static Rect? GetBoundsRelativeTo( this FrameworkElement element, UIElement otherElement )
		{
			if ( element == null )
				throw new ArgumentNullException( "element" );
			if ( otherElement == null )
				throw new ArgumentNullException( "otherElement" );

			try
			{
				Point origin, bottom;
				GeneralTransform transform = element.TransformToVisual( otherElement );

				if ( transform != null &&
					 transform.TryTransform( new Point(), out origin ) &&
					 transform.TryTransform( new Point( element.ActualWidth, element.ActualHeight ), out bottom ) )
				{
					return new Rect( origin, bottom );
				}
			}
			catch ( ArgumentException )
			{
				// Ignore any exceptions thrown while trying to transform
			}

			return null;
		}

		/// <summary>
		/// Perform an action when the element's LayoutUpdated event fires.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <param name="action">The action to perform.</param>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="element"/> is null.
		/// </exception>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="action"/> is null.
		/// </exception>
		public static void InvokeOnLayoutUpdated( this FrameworkElement element, Action action )
		{
			if ( element == null )
				throw new ArgumentNullException( "element" );
			if ( action == null )
				throw new ArgumentNullException( "action" );

			// Create an event handler that unhooks itself before calling the
			// action and then attach it to the LayoutUpdated event.
			EventHandler handler = null;
			handler = ( s, e ) =>
			{
				element.LayoutUpdated -= handler;
				action();
			};
			element.LayoutUpdated += handler;
		}

		/// <summary>
		/// Retrieves all the logical children of a framework element using a 
		/// breadth-first search. For performance reasons this method manually 
		/// manages the stack instead of using recursion.
		/// </summary>
		/// <param name="parent">The parent framework element.</param>
		/// <returns>The logical children of the framework element.</returns>
		internal static IEnumerable<FrameworkElement> GetLogicalChildren( this FrameworkElement parent )
		{
			Debug.Assert( parent != null, "The parent cannot be null." );

			Popup popup = parent as Popup;
			if ( popup != null )
			{
				var popupChild = popup.Child as FrameworkElement;
				if ( popupChild != null )
				{
					yield return popupChild;
				}
			}

			// If control is an items control return all children using the 
			// Item container generator.
			var itemsControl = parent as ItemsControl;
			if ( itemsControl != null )
			{
				foreach ( FrameworkElement logicalChild in
					Enumerable
						.Range( 0, itemsControl.Items.Count )
						.Select( index => itemsControl.ItemContainerGenerator.ContainerFromIndex( index ) )
						.OfType<FrameworkElement>() )
				{
					yield return logicalChild;
				}
			}

			string parentName = parent.Name;
			var queue = new Queue<FrameworkElement>( parent.GetVisualChildren().OfType<FrameworkElement>() );

			while ( queue.Count > 0 )
			{
				FrameworkElement element = queue.Dequeue();
				if ( element.Parent == parent || element is UserControl )
				{
					yield return element;
				}
				else
				{
					foreach ( var visualChild in element.GetVisualChildren().OfType<FrameworkElement>() )
						queue.Enqueue( visualChild );
				}
			}
		}
	}

	public static class ApplicationExtensions
	{
		public static UIElement GetShell( this System.Windows.Application target )
		{
			Contract.Requires( target != null );

			var result = target.GetShell<UIElement>();
			return result;
		}

		public static Uri GetUri( this System.Windows.Application target )
		{
			Contract.Requires( target != null );
			var result = target.MainWindow.As<NavigationWindow>().Transform( item => item.Source );
			return result;
		}

		public static Uri GetHostUri( this System.Windows.Application target )
		{
			Contract.Requires( target != null );
			var result = target.GetUri();
			return result;
		}

		public static TShell GetShell<TShell>( this System.Windows.Application target ) where TShell : UIElement
		{
			Contract.Requires( target != null );

			var result = target.MainWindow.As<TShell>();
			return result;
		}

		public static TService GetService<TService>( this System.Windows.Application target )
		{
			Contract.Requires( target != null );
			Contract.Requires( target.Resources != null );

			var result = target.Resources.Values.FirstOrDefaultOfType<TService>();
			return result;
		}
	}

	public static partial class FrameworkElementExtensions
	{
		public static bool IsAttachedToScene( this FrameworkElement target )
		{
			var root = target.GetVisualAncestorsAndSelf().LastOrDefault();
			var result = target.IsLoaded && ( root is ChildWindow || root is Window );
			return result;
		}
	}
	class CallbackContext<TItem> where TItem : class
	{
		readonly TItem item;
		readonly Delegate callback;

		public CallbackContext( TItem item, Delegate callback )
		{
			this.item = item;
			this.callback = callback;
		}

		public override bool Equals(object obj)
		{
			return obj is CallbackContext<TItem> && Equals( (CallbackContext<TItem>)obj );
		}

		bool Equals( CallbackContext<TItem> obj )
		{
			return Equals( obj.item, item ) && Equals( obj.callback, callback );
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return ( ( item != null ? item.GetHashCode() : 0 ) * 397 ) ^ ( callback != null ? callback.GetHashCode() : 0 );
			}
		}
	}

	public static partial class FrameworkElementExtensions
	{
		static readonly List<WeakReference<CallbackContext<FrameworkElement>>> Items = new List<WeakReference<CallbackContext<FrameworkElement>>>();

		public static bool IsVisibleInTree( this FrameworkElement target )
		{
			var result = target.Loop(
				item => VisualTreeHelper.GetParent( item ).As<FrameworkElement>(),
				item => item.Visibility != Visibility.Visible,
				item => false,
				true );
			return result;
		}

		public static void RefreshValue( this DependencyObject target, DependencyProperty property )
		{
			var current = target.GetValue( property );
			target.ClearValue( property );
			target.SetValue( property, current );
		}


		public static void EnsureLoaded<TFrameworkElement>( this TFrameworkElement target, Action<TFrameworkElement> callback, bool immediateExecution = true ) where TFrameworkElement : FrameworkElement
		{
			Action<TFrameworkElement> action = x => x.IsLoaded.IsTrue( () => callback( x ) );

			if ( target.IsLoaded )
			{
				var call = immediateExecution ? (Func<Action, IDelegateContext>)Threading.Application.Execute : Threading.Application.Start;
				call( () => action( target ) );
			}
			else
			{
				var key = new CallbackContext<FrameworkElement>( target, callback );
				Items.CheckWith( key, x =>
				{
					target.Loaded += new Loader<TFrameworkElement>( action ).OnLoad;
				} );
			}
		}

		readonly static IDictionary<Type,IEnumerable<DependencyProperty>> Properties = new Dictionary<Type, IEnumerable<DependencyProperty>>();

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Using Tuples for data contracts." )]
		public static IEnumerable<Tuple<DependencyProperty,BindingExpression>> GetAllBindings( this FrameworkElement target )
		{
			var result = Properties.Ensure( target.GetType(), x => x.GetFields( BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy ).Where( y => typeof(DependencyProperty).IsAssignableFrom( y.FieldType ) ).Select( y => y.GetValue( null ).To<DependencyProperty>() ) ).Select( y => target.GetBindingExpression( y ).Transform( z => new Tuple<DependencyProperty, BindingExpression>( y, z ) ) ).NotNull().ToArray();
			return result;
		}

		/*public static bool IsLoaded<TFrameworkElement>( this TFrameworkElement target )
				where TFrameworkElement : DependencyObject
		{
			var result = target.GetValueByHierarchy<bool>( Caliburn.Micro.View.IsLoadedProperty );
			return result;
		}*/

		class Loader<TFrameworkElement> where TFrameworkElement : FrameworkElement
		{
			readonly Action<TFrameworkElement> callback;

			public Loader( Action<TFrameworkElement> callback )
			{
				this.callback = callback;
			}

			public void OnLoad( object sender, EventArgs args )
			{
				var element = sender.To<TFrameworkElement>();
				callback( element );
				element.Loaded -= OnLoad;
			}
		}

		public static IEnumerable<TResult> GetAll<TResult>( this DependencyObject target, DependencyProperty property ) where TResult : class
		{
			var result = new List<TResult>();
			DependencyObjectExtensions.SearchFor( result, item => item.GetValue( property ).As<TResult>(), target );
			return result;
		}

		public static void SetBounds( this FrameworkElement target, Rect bounds )
		{
			SetBounds( target, bounds, null );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "z", Justification = "It is what it is." )]
		public static void SetBounds( this FrameworkElement target, Rect bounds, int? zIndex )
		{
			if ( zIndex != null )
			{
				Canvas.SetZIndex( target, zIndex.Value );
			}
			Canvas.SetLeft( target, bounds.X );
			Canvas.SetTop( target, bounds.Y );
			target.Width = bounds.Width;
			target.Height = bounds.Height;
		}

		public static TParent GetParentOfType<TParent>( this FrameworkElement target ) where TParent : class
		{
			var current = target.ResolveParent();
			while ( current != null )
			{
				var parent = current.As<TParent>();
				if ( parent != null )
				{
					return parent;
				}
				current = current.ResolveParent();
			}
			return null;
		}

		public static TParent GetRootOfType<TParent>( this FrameworkElement target ) where TParent : FrameworkElement
		{
			var current = target.GetParentOfType<TParent>();
			while ( current != null )
			{
				var parent = current.GetParentOfType<TParent>();
				if ( parent == null )
				{
					return current;
				}
				current = parent;
			}
			return null;
		}

		public static DependencyObject GetParentOfType( this FrameworkElement target, Type type )
		{
			var current = target.ResolveParent();
			while ( current != null )
			{
				if ( type.IsAssignableFrom( current.GetType() ) )
				{
					return current;
				}
				current = current.ResolveParent();
			}
			return null;
		}

		public static FrameworkElement ResolveParent( this FrameworkElement target )
		{
			var result = target.ResolveParent<FrameworkElement>();
			return result;
		}

		public static TParent ResolveParent<TParent>( this FrameworkElement target ) where TParent : FrameworkElement
		{
			var parent = target.Transform( x => x.Parent ?? VisualTreeHelper.GetParent( x ) );
			var result = parent.As<TParent>();
			return result;
		}

		public static IEnumerable<TControl> GetAllChildren<TControl>( this FrameworkElement target ) where TControl : class
		{
			var result = new List<TControl>();
			DependencyObjectExtensions.SearchFor( result, item => item != target ? item.As<TControl>() : null, target );
			return result;
		}

		public static bool IsInTree( this FrameworkElement target, FrameworkElement parent )
		{
			var current = target.ResolveParent();
			while ( current != null )
			{
				if ( current == parent )
				{
					return true;
				}
				current = current.ResolveParent();
			}
			return false;
		}
	}

	public static partial class DependencyObjectExtensions
	{
		static readonly List<WeakReference<CallbackContext<IAttachedObject>>> Items = new List<WeakReference<CallbackContext<IAttachedObject>>>();

		public static void SetProperty( this DependencyObject target, DependencyProperty property, object value, Type targetType = null )
		{
			Threading.Application.Execute( () =>
			{
				var checkedValue = /*value.AsTo<Binding,Binding>( x => x.ConnectedTo( target ) ) ??*/ targetType.Transform( value.ConvertTo, () => value );
				target.SetValue( property, checkedValue );
			} );
		}

		public static bool EnsureLoadedElement( this object target, Action<FrameworkElement> action )
		{
			var attached = target.As<Presentation.IAttachedObject>( x =>
			{
				if ( x.AssociatedObject != null )
				{
					Threading.Application.Execute( () => Ensure( x, action ) );
				}
				else
				{
					var key = new CallbackContext<IAttachedObject>( x, action );
					Items.CheckWith( key, y => x.Attached += new Loader( z => Ensure( x, action ) ).OnAttached );
				}
			} );
			var result = attached != null || Ensure( target, action ) != null;
			return result;
		}

		static FrameworkElement Ensure( object x, Action<FrameworkElement> action )
		{
			var result = x.DetermineFrameworkElement();
			result.EnsureLoaded( action );
			return result;
		}

		class Loader
		{
			readonly Action<Presentation.IAttachedObject> callback;

			public Loader( Action<Presentation.IAttachedObject> callback )
			{
				this.callback = callback;
			}

			public void OnAttached( object sender, EventArgs args )
			{
				var element = sender.To<Presentation.IAttachedObject>();
				callback( element );
				element.Attached -= OnAttached;
			}
		}

		public static FrameworkElement DetermineFrameworkElement( this object target )
		{
			var result = target.As<FrameworkElement>() ?? target.AsTo<IAttachedObject, FrameworkElement>( x => x.AssociatedObject.DetermineFrameworkElement() ) ?? target.AsTo<FrameworkContentElement, FrameworkElement>( x => x.FindLogicalAncestor<FrameworkElement>() );
			return result;
		}
	}

	public static partial class DependencyObjectExtensions
	{
		public static IEnumerable<T> FindVisualChildren<T>( this DependencyObject depObj ) where T : DependencyObject
		{
			if ( depObj != null )
			{
				for ( var i = 0; i < VisualTreeHelper.GetChildrenCount( depObj ); ++i )
				{
					var child = VisualTreeHelper.GetChild( depObj, i );
					var children = child as T;
					if ( children != null )
					{
						yield return children;
					}
					foreach ( T obj in child.FindVisualChildren<T>() )
					{
						yield return obj;
					}
				}
			}
		}

		public static IEnumerable<T> FindLogicalChildren<T>( this DependencyObject depObj ) where T : DependencyObject
		{
			if ( depObj != null )
			{
				foreach ( var depObj1 in LogicalTreeHelper.GetChildren( depObj ).OfType<DependencyObject>() )
				{
					var children = depObj1 as T;
					if ( children != null )
					{
						yield return children;
					}
					foreach ( T obj in depObj1.FindLogicalChildren<T>() )
					{
						yield return obj;
					}
				}
			}
		}

		public static DependencyObject FindVisualTreeRoot( this DependencyObject initial )
		{
			var dependencyObject1 = initial;
			var dependencyObject2 = initial;
			for ( ; dependencyObject1 != null; dependencyObject1 = dependencyObject1 is Visual || dependencyObject1 is Visual3D ? VisualTreeHelper.GetParent( dependencyObject1 ) : LogicalTreeHelper.GetParent( dependencyObject1 ) )
			{
				dependencyObject2 = dependencyObject1;
			}
			return dependencyObject2;
		}

		public static T FindVisualAncestor<T>( this DependencyObject dependencyObject ) where T : class
		{
			var reference = dependencyObject;
			do
			{
				reference = VisualTreeHelper.GetParent( reference );
			}
			while ( reference != null && !( reference is T ) );
			return reference as T;
		}

		public static T FindLogicalAncestor<T>( this DependencyObject dependencyObject ) where T : class
		{
			var current = dependencyObject;
			do
			{
				var reference = current;
				current = LogicalTreeHelper.GetParent( current ) ?? VisualTreeHelper.GetParent( reference );
			}
			while ( current != null && !( current is T ) );
			return current as T;
		}
	
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
			var items = target.DetermineFrameworkElement().GetSelfAndAncestors().Skip( includeSelf ? 0 : 1 );
			var element = items.FirstOrDefault( x => x.ReadLocalValue( property ) != DependencyProperty.UnsetValue );
			var value = element.Transform( x => x.ReadLocalValue( property ).Transform( y => y.AsTo<BindingExpression, TPropertyType>( FromBinding<TPropertyType>, y.To<TPropertyType> ) ) );
			var result = where == null || where( value ) ? value : default(TPropertyType);
			return result;
			/*for ( var current = includeSelf ? target : LogicalTreeHelper.GetParent( target ); current != null; current = LogicalTreeHelper.GetParent( current ) )
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
			return default(TPropertyType);*/
		}

		static T FromBinding<T>( BindingExpression arg )
		{
			arg.UpdateSource();
			return (T)arg.ParentBinding.Source;
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
			var result = candidates.Select( x => type.GetField( x, BindingOptions.AllProperties ) ).FirstOrDefault( x => x != null ).Transform( x => x.GetValue( null ).As<DependencyProperty>() );
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
