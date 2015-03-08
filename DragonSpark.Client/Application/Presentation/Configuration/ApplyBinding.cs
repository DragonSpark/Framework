using System;
using System.Collections;
using System.Reflection;
using System.Windows.Markup;
using DragonSpark.Application.Presentation.Extensions;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.Configuration
{
    [ContentProperty( "Binding" )]
    public class ApplyBinding : MarkupExtension
    {
        public System.Windows.Data.Binding Binding { get; set; }

        public override object ProvideValue( IServiceProvider serviceProvider )
        {
            var provider = serviceProvider.Get<IProvideValueTarget>();
            var property = provider.TargetProperty.To<PropertyInfo>();
            var targetObject = provider.TargetObject;
            var index = targetObject.AsTo<IList, int?>( x => x.Count );
            Binding.ApplyTo( targetObject, x =>
            {
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
            } );
            return null;
        }
    }
}