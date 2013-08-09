using System;
using System.Windows.Markup;
using Microsoft.Practices.Unity;

namespace DragonSpark.IoC.Configuration
{
	[ContentProperty( "Instance" )]
	public class PolicyInstance : InjectionMember
	{
		// [TypeConverter( typeof(TypeNameConverter) )]
		public Type PolicyType { get; set; }

		public PolicyCreatorBase Instance { get; set; }

		public override Microsoft.Practices.Unity.InjectionMember Create( IUnityContainer container, Type targetType )
		{
			var policy = Instance.PolicyInstance;
			var result = new PolicyInjection( PolicyType ?? policy.GetType(), policy );
			return result;
		}
	}
}