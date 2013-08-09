using System;
using Microsoft.Practices.Unity;

namespace DragonSpark.IoC.Configuration
{
	public class InjectionMethod : InjectionMemberParameterBase
	{
		public string MethodName { get; set; }

		public override Microsoft.Practices.Unity.InjectionMember Create( IUnityContainer container, Type targetType )
		{
			var result = new Microsoft.Practices.Unity.InjectionMethod( MethodName, Parameters.Resolve( targetType ) );
			return result;
		}
	}
}