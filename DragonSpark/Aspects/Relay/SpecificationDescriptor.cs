using DragonSpark.Aspects.Build;

namespace DragonSpark.Aspects.Relay
{
	public sealed class SpecificationDescriptor : Descriptor<SpecificationRelayAspect>
	{
		public static SpecificationDescriptor Default { get; } = new SpecificationDescriptor();
		SpecificationDescriptor() : base( GeneralizedSpecificationTypeDefinition.Default, GenericSpecificationTypeDefinition.Default, typeof(SpecificationRelay<>), typeof(ISpecificationRelay),
										  new MethodBasedAspectInstanceLocator<SpecificationMethodAspect>( GeneralizedSpecificationTypeDefinition.Default.Method )
		) {}
	}
}