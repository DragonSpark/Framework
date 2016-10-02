namespace DragonSpark.Aspects.Implementations
{
	public sealed class ParameterizedSourceDescriptor : Descriptor<GeneralizedParameterizedSourceAspect>
	{
		public static ParameterizedSourceDescriptor Default { get; } = new ParameterizedSourceDescriptor();
		ParameterizedSourceDescriptor() : base( ParameterizedSourceTypeDefinition.Default.DeclaringType, GeneralizedParameterizedSourceTypeDefinition.Default.DeclaringType ) {}
	}
}