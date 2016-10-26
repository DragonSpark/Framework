using System;

namespace DragonSpark.Application.Setup
{
	public struct ServiceRegistration
	{
		public ServiceRegistration( Type type, object instance )
		{
			RegistrationType = type;
			Instance = instance;
		}

		public object Instance { get; }

		public Type RegistrationType { get; }
	}
}