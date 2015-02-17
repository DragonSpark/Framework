using System;
using System.Collections;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Xaml;
using DragonSpark.Client.Windows.Extensions;
using DragonSpark.Extensions;

namespace DragonSpark.Client.Windows.Presentation
{
	public class BindExtension : MarkupExtension
	{
		readonly Binding binding;

		public BindExtension( Binding binding )
		{
			this.binding = binding;
		}

		public override object ProvideValue( IServiceProvider serviceProvider )
		{
			var service = serviceProvider.Get<IProvideValueTarget>();

			var result = service.TargetProperty.AsTo<PropertyInfo, bool>( x => typeof(Binding).IsAssignableFrom( x.PropertyType ) ) ? binding : Apply( serviceProvider ); 

			return result;
		}

		object Apply( IServiceProvider serviceProvider )
		{
			var rootObject = serviceProvider.Get<IRootObjectProvider>().RootObject;
			var provider = serviceProvider.Get<IProvideValueTarget>();
			var property = provider.TargetProperty.To<PropertyInfo>();
			var targetObject = provider.TargetObject;
			rootObject.As<DependencyObject>( root => binding.ApplyTo( root, x =>
			{
				var index = targetObject.AsTo<IList, int?>( y => y.Count );
				if ( index.HasValue )
				{
					targetObject.As<IList>( y =>
					{
						y.RemoveAt( index.Value );
						y.Insert( index.Value, x );
					} );
				}
				else
				{
					property.SetValue( targetObject, x, null );
				}
			} ) );
			var result = property.GetValue( targetObject );
			return result;
		}
	}
}
