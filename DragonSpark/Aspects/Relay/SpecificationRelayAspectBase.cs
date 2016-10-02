namespace DragonSpark.Aspects.Relay
{
	public abstract class SpecificationRelayAspectBase : RelayAspectBase, ISpecificationRelay
	{
		readonly ISpecificationRelay relay;

		protected SpecificationRelayAspectBase() {}

		protected SpecificationRelayAspectBase( ISpecificationRelay relay )
		{
			this.relay = relay;
		}

		public bool IsSatisfiedBy( object parameter ) => relay.IsSatisfiedBy( parameter );
	}
}