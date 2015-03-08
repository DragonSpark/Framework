using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Interactivity;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.Extensions
{
    public static partial class DependencyObjectExtensions
    {
        static readonly List<WeakReference> Items = new List<WeakReference>();

        /*public static void SetProperty( this DependencyObject target, DependencyProperty property, object value, Type targetType = null )
		{
			Threading.Application.Execute( () =>
			{
				var checkedValue = /*value.AsTo<Binding,Binding>( x => x.ConnectedTo( target ) ) ??#1# targetType.Transform( x => value.ConvertTo( x ), () => value );
				target.SetValue( property, checkedValue );
			} );
		}*/

        public static bool EnsureLoadedElement( this object target, Action<FrameworkElement> action )
        {
            var attached = target.As<ComponentModel.IAttachedObject>( x =>
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
            var result = attached != null || target.As<FrameworkElement>( x => Ensure( x, action ) ) != null;
            return result;
        }

        static void Ensure( object x, Action<FrameworkElement> action )
        {
            x.DetermineFrameworkElement().EnsureLoaded( action );
        }

        class Loader
        {
            readonly Action<ComponentModel.IAttachedObject> callback;

            public Loader( Action<ComponentModel.IAttachedObject> callback )
            {
                this.callback = callback;
            }

            public void OnAttached( object sender, EventArgs args )
            {
                var element = sender.To<ComponentModel.IAttachedObject>();
                callback( element );
                element.Attached -= OnAttached;
            }
        }

        public static FrameworkElement DetermineFrameworkElement( this object target )
        {
            var result = target.As<FrameworkElement>() ?? target.AsTo<IAttachedObject, FrameworkElement>( x => x.AssociatedObject.DetermineFrameworkElement() );
            return result;
        }
    }
}