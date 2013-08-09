using System;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;

namespace DragonSpark.IoC.Configuration
{
	public class PolicyReference : InjectionMember
	{
		// [TypeConverter( typeof(TypeNameConverter) )]
		public Type PolicyType { get; set; }

		public string BuildName { get; set; }

		// [TypeConverter( typeof(TypeNameConverter) )]
		public Type BuildType { get; set; }

		public override Microsoft.Practices.Unity.InjectionMember Create( IUnityContainer container, Type targetType )
		{
			var policy = CreatePolicy( container );
			var result = new PolicyInjection( PolicyType ?? BuildType, policy );
			return result;
		}

		public IBuilderPolicy CreatePolicy( IUnityContainer container )
		{
			using ( var child = container.CreateChildContainer() )
			{
				var result = (IBuilderPolicy)child.Resolve( BuildType, BuildName );
				return result;
			}
		}
	}
}