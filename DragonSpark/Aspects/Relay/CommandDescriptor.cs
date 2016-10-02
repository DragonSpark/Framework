using DragonSpark.Aspects.Build;

namespace DragonSpark.Aspects.Relay
{
	public sealed class CommandDescriptor : Descriptor<CommandRelayAspect>
	{
		public static CommandDescriptor Default { get; } = new CommandDescriptor();
		CommandDescriptor() : base( CommandTypeDefinition.Default, GenericCommandTypeDefinition.Default, typeof(CommandRelay<>), typeof(ICommandRelay),
									new MethodBasedAspectInstanceLocator<SpecificationMethodAspect>( CommandTypeDefinition.Default.Validation ),
									new MethodBasedAspectInstanceLocator<CommandMethodAspect>( CommandTypeDefinition.Default.Execution )
		) {}
	}
}