using System;

namespace DragonSpark.Activation.IoC.Commands
{
	[AttributeUsage( AttributeTargets.Class )]
	public sealed class LifetimeManagerAttribute : Attribute
	{
		public Type LifetimeManagerType { get; private set; }

		public LifetimeManagerAttribute( Type lifetimeManagerType )
		{
			LifetimeManagerType = lifetimeManagerType;
		}
	}
}