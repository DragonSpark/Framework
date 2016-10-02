using PostSharp.Aspects.Advices;

namespace DragonSpark.Aspects.Relay
{
	[IntroduceInterface( typeof(IParameterizedSourceRelay) )]
	public sealed class SourceRelayAspect : RelayAspectBase, IParameterizedSourceRelay
	{
		readonly IParameterizedSourceRelay relay;

		public SourceRelayAspect() {}

		public SourceRelayAspect( IParameterizedSourceRelay relay )
		{
			this.relay = relay;
		}

		public object Get( object parameter ) => relay.Get( parameter );
	}
}