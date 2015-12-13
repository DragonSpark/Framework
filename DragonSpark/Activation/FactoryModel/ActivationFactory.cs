namespace DragonSpark.Activation.FactoryModel
{
	public static class Factory
	{
		public static T Create<T>( object context = null )
		{
			var type = FactoryReflectionSupport.Instance.GetFactoryType( typeof(T) );
			var result = (T)new FactoryBuiltObjectFactory().Create( new ObjectFactoryParameter( type, context ) );
			return result;
		}
	}

	public abstract class ActivationFactory<TParameter, TResult> : FactoryBase<TParameter, TResult> where TParameter : ActivationParameter where TResult : class
	{
		/*protected ActivationFactory() : this( SystemActivator.Instance )
		{}*/

		protected ActivationFactory( IActivator activator ) : this( activator, new FactoryParameterCoercer<TParameter>( activator ) )
		{}

		protected ActivationFactory( IActivator activator, IFactoryParameterCoercer<TParameter> coercer ) : base( coercer )
		{
			Activator = activator;
		}

		protected IActivator Activator { get; }

		protected override TResult CreateItem( TParameter parameter )
		{
			var result = Activate( parameter );
			return result;
		}

		/*protected virtual Type DetermineType( TParameter parameter )
		{
			return parameter.Type;
		}*/

		protected abstract TResult Activate( TParameter parameter );
	}
}