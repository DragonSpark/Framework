namespace DragonSpark.Activation.FactoryModel
{
	public class ConstructFactory<TResult> : ActivationFactory<ConstructParameter, TResult> where TResult : class
	{
		public ConstructFactory() : this( SystemActivator.Instance )
		{}

		public ConstructFactory( IActivator activator ) : this( activator, new ConstructFactoryParameterCoercer<TResult>( activator ) )
		{}

		public ConstructFactory( IActivator activator, IFactoryParameterCoercer<ConstructParameter> coercer ) : base( activator, coercer )
		{}

		protected override TResult Activate( ConstructParameter parameter )
		{
			var result = Activator.Construct<TResult>( parameter.Type, parameter.Arguments );
			return result;
		}
	}

	public abstract class ActivateFactory<TParameter, TResult> : ActivationFactory<TParameter, TResult> where TResult : class where TParameter : ActivateParameter
	{
		protected ActivateFactory( IActivator activator ) : base( activator )
		{}

		protected ActivateFactory( IActivator activator, IFactoryParameterCoercer<TParameter> coercer ) : base( activator, coercer )
		{}

		protected override TResult Activate( TParameter parameter )
		{
			var result = Activator.Activate<TResult>( parameter.Type, parameter.Name );
			return result;
		}
	}
}