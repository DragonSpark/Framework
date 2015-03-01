using DragonSpark.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace DragonSpark.Application.Presentation.Extensions
{
    public static partial class FrameworkElementExtensions
	{
		static readonly List<WeakReference> Items = new List<WeakReference>();

		public static bool IsVisibleInTree( this FrameworkElement target )
		{
			var result = target.ResolveFromParent(
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
			Action<TFrameworkElement> action = x => x.IsAttachedToScene().IsTrue( () => callback( x ) );

			if ( target.IsAttachedToScene() )
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
}