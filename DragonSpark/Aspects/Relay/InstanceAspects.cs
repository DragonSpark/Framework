using PostSharp.Aspects;

namespace DragonSpark.Aspects.Relay
{
	public sealed class InstanceAspects : AdapterLocatorBase<IAspect>
	{
		public static InstanceAspects Default { get; } = new InstanceAspects();
		InstanceAspects() : base( Descriptors.Default.Aspects ) {}	
	}
}