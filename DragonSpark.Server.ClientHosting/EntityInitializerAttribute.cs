using System;

namespace DragonSpark.Server.ClientHosting
{
	[AttributeUsage( AttributeTargets.Method )]
	public class EntityInitializerAttribute : Attribute
	{}
}