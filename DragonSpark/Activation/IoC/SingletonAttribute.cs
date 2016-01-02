using System;

namespace DragonSpark.Activation.IoC
{
	[AttributeUsage( AttributeTargets.Property )]
	public class SingletonAttribute : Attribute
	{}
}