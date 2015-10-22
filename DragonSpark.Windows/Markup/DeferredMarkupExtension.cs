using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Markup;
using DragonSpark.Activation;
using DragonSpark.Extensions;

// Based on/Credit: http://blogs.msdn.com/b/ifeanyie/archive/2010/03/27/9986217.aspx
namespace DragonSpark.Windows.Markup
{
	public abstract class DeferredMarkupExtension : MarkupExtension
	{
		static readonly IMarkupTargetValueSetterBuilder[] DefaultBuilders = 
		{
			DependencyPropertyMarkupTargetValueSetterBuilder.Instance,
			CollectionTargetSetterBuilder.Instance,
			PropertyInfoMarkupTargetValueSetterBuilder.Instance,
			FieldInfoMarkupTargetValueSetterBuilder.Instance
		};

		protected DeferredMarkupExtension()
		{
			builders = new Lazy<IMarkupTargetValueSetterBuilder[]>( DetermineBuilders );
		}

		public override sealed object ProvideValue( IServiceProvider serviceProvider )
		{
			var result = !DesignerProperties.GetIsInDesignMode( new DependencyObject() ) ? Build( serviceProvider ) : null;
			return result;
		}

		object Build( IServiceProvider serviceProvider )
		{
			// Retrieve target information
			var service = serviceProvider.Get<IProvideValueTarget>();
			if ( service?.TargetObject != null )
			{
				// In a template the TargetObject is a SharedDp (internal WPF class)
				// In that case, the markup extension itself is returned to be re-evaluated later
				switch ( service.TargetObject.GetType().FullName )
				{
					case "System.Windows.SharedDp":
						return this;
					default:
						return Builders.FirstOrDefault( builder => builder.Handles( service ) )
							.Transform( builder => BeginProvideValue( Prepare( serviceProvider, service, builder ), builder.Create<IMarkupTargetValueSetter>( serviceProvider ) ) );
				}

				/*// Save target information for later updates
				TargetObject = target.TargetObject;
				TargetProperty = target.TargetProperty;*/
			}

			return null;
		}

		protected virtual IServiceProvider Prepare( IServiceProvider serviceProvider, IProvideValueTarget target, IMarkupTargetValueSetterBuilder builder )
		{
			var result = new DeferredContext( serviceProvider, target.TargetObject, target.TargetProperty, builder.GetPropertyType( target ) );
			return result;
		}

		/*protected object TargetObject { get; set; }

		protected object TargetProperty { get; set; }*/

		protected abstract object BeginProvideValue( IServiceProvider serviceProvider, IMarkupTargetValueSetter setter );

		protected IMarkupTargetValueSetterBuilder[] Builders => builders.Value;
		readonly Lazy<IMarkupTargetValueSetterBuilder[]> builders;

		protected virtual IMarkupTargetValueSetterBuilder[] DetermineBuilders()
		{
			return DefaultBuilders;
		}
	}

	public class DeferredContext : IServiceProvider
	{
		readonly IServiceProvider inner;

		public DeferredContext( IServiceProvider inner, object targetObject, object targetProperty, Type propertyType )
		{
			this.inner = inner;
			TargetObject = targetObject;
			TargetProperty = targetProperty;
			PropertyType = propertyType;
		}

		public object TargetObject { get; }
		public object TargetProperty { get; }
		public Type PropertyType { get; }

		public virtual object GetService( Type serviceType )
		{
			var result = typeof(DeferredContext).IsAssignableFrom( serviceType ) ? this : inner.GetService( serviceType );
			return result;
		}
	}
}
