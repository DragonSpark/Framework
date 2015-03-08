using System;
using System.Windows.Markup;
using DragonSpark.Objects;

namespace DragonSpark.IoC.Configuration
{
	[ContentProperty( "Factory" )]
	public class FactoryParameterValue : InjectionParameterValue
	{
		public IFactory Factory { get; set; }

		public override Microsoft.Practices.Unity.InjectionParameterValue Create( Type targetType )
		{
			var result = new IoC.FactoryParameterValue( Factory, targetType );
			return result;
		}
	}
}