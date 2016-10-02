using System;
using PostSharp.Aspects;
using PostSharp.Aspects.Configuration;
using PostSharp.Aspects.Serialization;

namespace DragonSpark.Aspects.Relay
{
	[AttributeUsage( AttributeTargets.Class ), AspectConfiguration( SerializerType = typeof(MsilAspectSerializer) )]
	public abstract class RelayAspectBase : InstanceLevelAspect
	{
		public override object CreateInstance( AdviceArgs adviceArgs ) => InstanceAspects.Default.Get( adviceArgs.Instance );
	}
}