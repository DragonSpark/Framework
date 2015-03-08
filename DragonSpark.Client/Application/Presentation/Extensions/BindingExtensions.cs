using System;
using System.Collections.Generic;
using System.Windows.Data;
using DragonSpark.Extensions;
using Expression.Samples.Interactivity.DataHelpers;

namespace DragonSpark.Application.Presentation.Extensions
{
    public static class BindingExtensions
	{
		readonly static IList<WeakReference> References = new List<WeakReference>();

		public static void ApplyTo( this Binding target, object source, Action<object> setter )
		{
			References.CheckWith( target, x => source.EnsureLoadedElement( y =>
			{
				new BindingListener( ( s, a ) => setter( a.EventArgs.NewValue ) ) { Binding = target, Element = y };
			} ) );
		}
		/*readonly static IList<WeakReference> References = new List<WeakReference>();

		public static object DetermineValue( this IAttachedObject target, object value, Type targetType = null )
		{
			var result = value.AsTo<Binding,Binding>( x => x.ConnectedTo( target.AssociatedObject ) ) ?? targetType.Transform( x => value.ConvertTo( x ), () => value );
			return result;
		}

		public static Binding ConnectedTo( this Binding target, DependencyObject host )
		{
			References.CheckWith( target, x =>
			{
				
			} );

			var result = host.AsTo<IAttachedObject, Binding>( x =>
			{
				var exists = References.AliveOnly().Exists( target );
				var item = exists ? target : new BindingListener { Binding = target, Element = x.AssociatedObject.To<FrameworkElement>() }.Binding;
				exists.IsFalse( () => References.Add( new WeakReference( target ) ) );
				return item;
			} ) ?? target;
			return result;
		}

		public static Binding CreateBinding( this Type target, string name, object source = null )
		{
			var markup = String.Format( "<Binding xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation\' xmlns:ns='clr-namespace:{0};assembly={1}' Path='(ns:{2}.{3})' />", target.Namespace, target.Assembly.GetAssemblyName(), target.Name, name );
			var result = (Binding)XamlReader.Load( markup );
			result.Source = source;
			return result;
		}*/
	}
}