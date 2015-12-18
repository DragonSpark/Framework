using DragonSpark.Extensions;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Markup;

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

		public sealed override object ProvideValue( IServiceProvider serviceProvider )
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
						return Builders.FirstOrDefault( builder => builder.Handles( service ) ).With( builder =>
						{
							var provider = Prepare( serviceProvider, service, builder );
							var setter = builder.Create( service );
							var value = BeginProvideValue( provider, setter );
							return value;
						} );
				}
			}

			return null;
		}

		protected virtual IServiceProvider Prepare( IServiceProvider serviceProvider, IProvideValueTarget target, IMarkupTargetValueSetterBuilder builder )
		{
			var result = new DeferredContext( serviceProvider, target.TargetObject, target.TargetProperty, builder.GetPropertyType( target ) );
			return result;
		}

		protected abstract object BeginProvideValue( IServiceProvider serviceProvider, IMarkupTargetValueSetter setter );

		protected IMarkupTargetValueSetterBuilder[] Builders => builders.Value;
		readonly Lazy<IMarkupTargetValueSetterBuilder[]> builders;

		protected virtual IMarkupTargetValueSetterBuilder[] DetermineBuilders()
		{
			return DefaultBuilders;
		}
	}
}
