using PostSharp.Aspects.Advices;

namespace DragonSpark.Aspects.Relay
{
	[IntroduceInterface( typeof(ICommandRelay) )]
	public sealed class CommandRelayAspect : SpecificationRelayAspectBase, ICommandRelay
	{
		readonly ICommandRelay relay;

		public CommandRelayAspect() {}

		public CommandRelayAspect( ICommandRelay relay ) : base( relay )
		{
			this.relay = relay;
		}

		public void Execute( object parameter ) => relay.Execute( parameter );
	}
}