using DragonSpark.Extensions;
using DragonSpark.Sources.Parameterized;
using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Markup;

namespace DragonSpark.Windows.Markup
{
	public abstract class MarkupExtensionBase : MarkupExtension
	{
		readonly static Func<IServiceProvider, IMarkupProperty> Setter = MarkupValueSetterFactory.Default.ToSourceDelegate();
		readonly static Func<Type, object> DesignTime = DesignTimeValueProvider.Default.ToSourceDelegate();

		readonly static ThreadLocal<DependencyObject> DependencyObject = new ThreadLocal<DependencyObject>( () => new DependencyObject() );

		readonly Func<IServiceProvider, IMarkupProperty> factory;
		readonly Func<Type, object> designTimeFactory;

		protected MarkupExtensionBase() : this( Setter, DesignTime ) {}

		protected MarkupExtensionBase( Func<Type, object> designTimeFactory ) : this( Setter, designTimeFactory ) {}

		protected MarkupExtensionBase( Func<IServiceProvider, IMarkupProperty> factory, Func<Type, object> designTimeFactory )
		{
			this.factory = factory;
			this.designTimeFactory = designTimeFactory;
		}

		public override object ProvideValue( IServiceProvider serviceProvider )
		{
			var designMode = DesignerProperties.GetIsInDesignMode( DependencyObject.Value );
			try
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
							var property = factory( serviceProvider );
							if ( property != null )
							{
								var value = designMode ? designTimeFactory( property.Reference.PropertyType ) : GetValue( new MarkupServiceProvider( serviceProvider, service.TargetObject, property ) );
								var result = service.TargetProperty == null ? property.SetValue( value ) : value;
								return result;
							}
							break;
					}
				}
				return null;
			}
			catch ( Exception e )
			{
				var exception = designMode ? new Exception( e.ToString() ) : e;
				throw exception;
			}
		}

		protected abstract object GetValue( MarkupServiceProvider serviceProvider );
	}
}