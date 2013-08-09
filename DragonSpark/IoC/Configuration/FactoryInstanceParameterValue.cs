using DragonSpark.Objects;
using System;
using System.Windows.Markup;

namespace DragonSpark.IoC.Configuration
{
	[ContentProperty( "Factory" )]
	public class FactoryInstanceParameterValue : InjectionParameterValue
	{
		public IFactory Factory { get; set; }

		public object Parameter { get; set; }

		public override Microsoft.Practices.Unity.InjectionParameterValue Create( Type targetType )
		{
			var result = new IoC.FactoryInstanceParameterValue( Factory, Parameter, targetType );
			return result;
		}
	}

	[ContentProperty( "Key" )]
	public class FactoryParameterValue : InjectionParameterValue
	{
		public NamedTypeBuildKey Key { get; set; }

		public object Parameter { get; set; }

		public override Microsoft.Practices.Unity.InjectionParameterValue Create( Type targetType )
		{
			var result = new IoC.FactoryParameterValue( Key, Parameter, targetType );
			return result;
		}
	}
}