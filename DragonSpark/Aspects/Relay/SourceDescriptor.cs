using DragonSpark.Aspects.Build;

namespace DragonSpark.Aspects.Relay
{
	public sealed class SourceDescriptor : Descriptor<SourceRelayAspect>
	{
		public static SourceDescriptor Default { get; } = new SourceDescriptor();
		SourceDescriptor() : base( GeneralizedParameterizedSourceTypeDefinition.Default, ParameterizedSourceTypeDefinition.Default, typeof(ParameterizedSourceRelay<,>), typeof(IParameterizedSourceRelay),
								   new MethodBasedAspectInstanceLocator<ParameterizedSourceMethodAspect>( GeneralizedParameterizedSourceTypeDefinition.Default.Method )
		) {}
	}
}