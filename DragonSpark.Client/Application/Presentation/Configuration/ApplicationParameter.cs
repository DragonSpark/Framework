using System;
using System.Reflection;
using System.Windows.Markup;
using DragonSpark.Extensions;
using DragonSpark.Runtime;

namespace DragonSpark.Application.Presentation.Configuration
{
    public class ApplicationParameter : Type
    {
        public string ParameterName { get; set; }

        protected override object ResolveValue( IServiceProvider serviceProvider, System.Type type )
        {
            var targetType = serviceProvider.GetService( typeof(IProvideValueTarget) ).To<IProvideValueTarget>().TargetProperty.AsTo<PropertyInfo,System.Type>( x => x.PropertyType );
            var result = ServiceLocation.With<IApplicationParameterSource, object>( x => x.Retrieve( new DragonSpark.Application.ApplicationParameter( type, ParameterName ) ).ConvertTo( targetType ) );
            return result;
        }
    }
}