using System;

namespace DragonSpark.Setup
{
	[AttributeUsage( AttributeTargets.Class )]
	public class LifetimeManagerAttribute : Attribute
	{
		public LifetimeManagerAttribute( Type lifetimeManagerType )
		{
			LifetimeManagerType = lifetimeManagerType;
		}

		public Type LifetimeManagerType { get; }
	}

	/*public class SingletonAttribute : Attribute
	{ }*/
}