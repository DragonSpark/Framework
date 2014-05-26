﻿using DragonSpark.Extensions;
using DragonSpark.Runtime;
using System;
using System.Configuration;
using System.Reflection;
using System.Windows.Markup;

namespace DragonSpark.Configuration
{
	public class ApplicationConfigurationExtension : MarkupExtension
	{
		readonly Type type;
		readonly string name;

		public ApplicationConfigurationExtension( string name ) : this( null, name )
		{}

		public ApplicationConfigurationExtension( Type type, string name )
		{
			this.type = type;
			this.name = name;
		}

		public object DefaultValue { get; set; }

		public override object ProvideValue( IServiceProvider serviceProvider )
		{
			var service = serviceProvider.Get<IProvideValueTarget>();
			var key = type.Transform( x => string.Concat( x.FullName, "::", name ), () => name );
			var source = ConfigurationManager.AppSettings[ key ] ?? DefaultValue;
			var result = source.ConvertTo( service.TargetProperty.To<PropertyInfo>().PropertyType );
			return result;
		}
	}
}