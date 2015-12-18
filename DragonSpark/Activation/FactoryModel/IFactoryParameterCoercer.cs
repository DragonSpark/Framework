namespace DragonSpark.Activation.FactoryModel
{
	public interface IFactoryParameterCoercer<out TParameter>
	{
		TParameter Coerce( object context );
	}
}