using System;
using Microsoft.Practices.Unity;

namespace DragonSpark.Activation.IoC
{
	public class InjectionMemberContext
	{
		public InjectionMemberContext( IUnityContainer container, Type targetType )
		{
			Container = container;
			TargetType = targetType;
		}

		public IUnityContainer Container { get; }

		public Type TargetType { get; }
	}
}