using PostSharp.Aspects.Advices;

namespace DragonSpark.Aspects.Relay
{
	[IntroduceInterface( typeof(ISpecificationRelay) )]
	public sealed class SpecificationRelayAspect : SpecificationRelayAspectBase
	{
		public SpecificationRelayAspect() {}
		public SpecificationRelayAspect( ISpecificationRelay relay ) : base( relay ) {}
	}
}