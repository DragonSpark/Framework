using System;

namespace DragonSpark.Application.Setup
{
	public struct InstanceRegistrationRequest
	{
		public InstanceRegistrationRequest( object instance ) : this( instance.GetType(), instance ) {}

		public InstanceRegistrationRequest( Type type, object instance )
		{
			RegistrationType = type;
			Instance = instance;
		}

		public object Instance { get; }

		public Type RegistrationType { get; }
	}
}