namespace DragonSpark.Aspects.Implementations
{
	public sealed class SpecificationDescriptor : Descriptor<GeneralizedSpecificationAspect>
	{
		public static SpecificationDescriptor Default { get; } = new SpecificationDescriptor();
		SpecificationDescriptor() : base( GenericSpecificationTypeDefinition.Default.DeclaringType, GeneralizedSpecificationTypeDefinition.Default.DeclaringType, CommandTypeDefinition.Default.DeclaringType ) {}
	}
}