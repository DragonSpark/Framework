using System;
using PostSharp.Aspects;
using PostSharp.Aspects.Configuration;
using PostSharp.Aspects.Serialization;

namespace DragonSpark.Aspects.Implementations
{
	[AttributeUsage( AttributeTargets.Class ), AspectConfiguration( SerializerType = typeof(MsilAspectSerializer) )]
	public abstract class GeneralizedAspectBase : InstanceLevelAspect {}
}