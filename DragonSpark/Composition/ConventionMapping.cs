using System;

namespace DragonSpark.Composition
{
	public struct ConventionMapping
	{
		public ConventionMapping( Type interfaceType, Type implementationType )
		{
			InterfaceType = interfaceType;
			ImplementationType = implementationType;
		}

		public Type InterfaceType { get; }
		public Type ImplementationType { get; }
	}
}