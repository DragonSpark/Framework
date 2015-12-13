using System;
using Microsoft.Practices.Unity;

namespace DragonSpark.Activation.IoC
{
	public class RegistrationSpecificationParameter
	{
		public RegistrationSpecificationParameter( IUnityContainer container, Type type )
		{
			Container = container;
			Type = type;
		}

		public IUnityContainer Container { get; }
		public Type Type { get; }
	}
}