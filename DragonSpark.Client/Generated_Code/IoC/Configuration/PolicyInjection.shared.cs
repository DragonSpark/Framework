using System;
using System.Diagnostics.Contracts;
using Microsoft.Practices.ObjectBuilder2;

namespace DragonSpark.IoC.Configuration
{
	public class PolicyInjection : Microsoft.Practices.Unity.InjectionMember
	{
		readonly Type policyType;

		public PolicyInjection( Type policyType, IBuilderPolicy policy )
		{
			Contract.Requires( policyType != null && typeof(IBuilderPolicy).IsAssignableFrom( policyType ) );
			Contract.Requires( policy != null );

			this.policyType = policyType;
			this.policy = policy;
		}

		/*[ContractInvariantMethod]
		void Invariant()
		{
			Contract.Invariant( policyType != null && typeof(IBuilderPolicy).IsAssignableFrom( policyType ) );
			Contract.Invariant( policy != null );
		}*/

		public IBuilderPolicy Policy
		{
			get { return policy; }
		}	readonly IBuilderPolicy policy;

		public override void AddPolicies(Type serviceType, Type implementationType, string name, IPolicyList policies)
		{
			policies.Set( policyType, policy, new Microsoft.Practices.ObjectBuilder2.NamedTypeBuildKey( implementationType, name ) );
		}
	}
}