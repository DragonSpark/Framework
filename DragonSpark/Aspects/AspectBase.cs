using PostSharp.Aspects;
using PostSharp.Aspects.Configuration;
using PostSharp.Aspects.Serialization;
using System;

namespace DragonSpark.Aspects
{
	[MethodInterceptionAspectConfiguration( SerializerType = typeof(MsilAspectSerializer) ), AttributeUsage( AttributeTargets.Method )]
	public abstract class AspectBase : MethodInterceptionAspect {}
}