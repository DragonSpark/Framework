using System;
using System.Windows.Markup;
using DragonSpark.Extensions;
using DragonSpark.Runtime;
using Microsoft.Practices.Unity;

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
			var value = ParameterType.Transform( item => instance.ConvertTo( item ), () => instance );
			var result = ParameterType.Transform( x => new InjectionParameter( x, value ), () => new InjectionParameter( value ) );
			return result;
		}
	}
}