namespace DragonSpark.Aspects.Relay
{
	public interface ICommandRelay : ISpecificationRelay
	{
		void Execute( object parameter );
	}
}