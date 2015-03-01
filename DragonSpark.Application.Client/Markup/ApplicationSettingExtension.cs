using System;
using System.Reflection;
using System.Windows.Markup;
using DragonSpark.ComponentModel;
using DragonSpark.Extensions;
using DragonSpark.Runtime;

namespace DragonSpark.Markup
{
	public class ApplicationSettingExtension : MarkupExtension
	{
		readonly Type type;
		readonly string name;

		public ApplicationSettingExtension( string name ) : this( null, name )
		{}

		public ApplicationSettingExtension( Type type, string name ) : this( type, name, null )
		{}

		public ApplicationSettingExtension( Type type, string name, object defaultValue )
		{
			this.type = type;
			this.name = name;
			DefaultValue = defaultValue;
		}

		public object DefaultValue { get; set; }

		public override object ProvideValue( IServiceProvider serviceProvider )
		{
			var service = serviceProvider.Get<IProvideValueTarget>();

			var result = type.Get( name, DefaultValue, service.TargetProperty.To<PropertyInfo>().PropertyType );
			return result;
		}
	}
}