namespace DragonSpark.Activation.FactoryModel
{
	public interface IFactoryParameterQualifier<out TParameter>
	{
		TParameter Qualify( object context );
	}
}