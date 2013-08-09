using System;
using System.Windows.Markup;

namespace DragonSpark.IoC.Configuration
{
	[ContentProperty( "OptionalType" )]
	public class OptionalParameter : InjectionParameterValue
	{
		// [TypeConverter( typeof(TypeNameConverter) )]
		public Type OptionalType { get; set; }

		public override Microsoft.Practices.Unity.InjectionParameterValue Create( Type targetType )
		{
			var result = new Microsoft.Practices.Unity.OptionalParameter( OptionalType );
			return result;
		}
	}
}