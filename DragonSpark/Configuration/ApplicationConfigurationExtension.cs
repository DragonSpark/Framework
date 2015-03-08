using System;
using System.Configuration;
using System.Reflection;
using System.Windows.Markup;
using DragonSpark.Extensions;
using DragonSpark.Runtime;

namespace DragonSpark.Configuration
{
	public class ApplicationConfigurationExtension : MarkupExtension
	{
		readonly string name;

		public ApplicationConfigurationExtension( string name )
		{
			this.name = name;
		}

		public object DefaultValue { get; set; }

		public override object ProvideValue( IServiceProvider serviceProvider )
		{
			var service = serviceProvider.GetService( typeof(IProvideValueTarget) ).To<IProvideValueTarget>();
			var source = ConfigurationManager.AppSettings[ name ] ?? DefaultValue;
			var result = source.ConvertTo( service.TargetProperty.To<PropertyInfo>().PropertyType );
			return result;
		}
	}
}