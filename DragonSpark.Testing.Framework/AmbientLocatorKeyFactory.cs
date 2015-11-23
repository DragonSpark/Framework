using System;
using System.Reflection;
using DragonSpark.Activation;
using DragonSpark.Runtime;
using Microsoft.Practices.ServiceLocation;

namespace DragonSpark.Testing.Framework
{
	public class AmbientLocatorKeyFactory : FactoryBase<MethodInfo, IAmbientKey>
	{
		public static AmbientLocatorKeyFactory Instance { get; } = new AmbientLocatorKeyFactory();

		protected override IAmbientKey CreateFrom( Type resultType, MethodInfo parameter )
		{
			var specification = new CurrentMethodSpecification( parameter );
			var result = new AmbientKey<IServiceLocator>( specification );
			return result;
		}
	}
}