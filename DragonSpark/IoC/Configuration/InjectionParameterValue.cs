using System;
using System.ComponentModel;

namespace DragonSpark.IoC.Configuration
{
	[TypeConverter( typeof(InjectionParameterValueTypeConverter) )]
	public abstract class InjectionParameterValue
	{
		public abstract Microsoft.Practices.Unity.InjectionParameterValue Create( Type targetType );
	}
}