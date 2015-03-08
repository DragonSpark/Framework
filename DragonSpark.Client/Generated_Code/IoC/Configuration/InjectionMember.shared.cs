using System;
using Microsoft.Practices.Unity;

namespace DragonSpark.IoC.Configuration
{
	public abstract class InjectionMember
	{
		public abstract Microsoft.Practices.Unity.InjectionMember Create( IUnityContainer container, Type targetType );
	}
}