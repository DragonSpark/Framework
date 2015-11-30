namespace DragonSpark.Activation.FactoryModel
{
	public interface IFactoryWithParameter
	{
		object Create( object parameter );

		/*Type ParameterType { get; }*/
	}
}