using System;

namespace DragonSpark.Runtime
{
	[AttributeUsage( AttributeTargets.Property )]
	public class SingletonInstanceAttribute : Attribute
	{}
}