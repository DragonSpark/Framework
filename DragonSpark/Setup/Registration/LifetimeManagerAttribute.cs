using System;
using Microsoft.Practices.Unity;

namespace DragonSpark.Setup.Registration
{
	public class PersistentAttribute : LifetimeManagerAttribute
	{
		public PersistentAttribute() : base( typeof(ContainerControlledLifetimeManager) ) {}
	}

	public class TransientAttribute : LifetimeManagerAttribute
	{
		public TransientAttribute() : base( typeof(TransientLifetimeManager) ) { }
	}

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