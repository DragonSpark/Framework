using System;
using Microsoft.Practices.Unity;

namespace DragonSpark.IoC.Configuration
{
	public class InjectionConstructor : InjectionMemberParameterBase
	{
		public override Microsoft.Practices.Unity.InjectionMember Create( IUnityContainer container, Type targetType )
		{
			var result = new Microsoft.Practices.Unity.InjectionConstructor( Parameters.Resolve( targetType ) );
			return result;
		}
	}
}