using DragonSpark.Runtime;
using Microsoft.Practices.Unity;
using System;
using System.Windows.Markup;

namespace DragonSpark.IoC.Configuration
{
	[ContentProperty( "Instance" )]
	public class InstanceValue : InjectionParameterValue
	{
		// [TypeConverter( typeof(TypeNameConverter) )]
		public Type ParameterType { get; set; }

		public object Instance { get; set; }

		protected virtual object ResolveInstance()
		{
			return Instance;
		}

		public override Microsoft.Practices.Unity.InjectionParameterValue Create( Type targetType )
		{
			var instance = ResolveInstance();
			var type = ParameterType ?? instance.GetType();
			var value = instance.ConvertTo( type );
			var result = new InjectionParameter( type, value );
			return result;
		}
	}
}