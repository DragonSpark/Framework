using System;

namespace DragonSpark.Setup.Registration
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