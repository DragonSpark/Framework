using PostSharp.Patterns.Threading;

namespace DragonSpark.Activation.FactoryModel
{
	// [Synchronized]
	public class ActivateFactory<TResult> : ActivateFactory<ActivateParameter, TResult> where TResult : class
	{
		public static ActivateFactory<TResult> Instance { get; } = new ActivateFactory<TResult>();

		public ActivateFactory() : this( SystemActivator.Instance )
		{}

		public ActivateFactory( IActivator activator ) : this( activator, new ActivateFactoryParameterCoercer<TResult>( activator ) )
		{}

		public ActivateFactory( IActivator activator, IFactoryParameterCoercer<ActivateParameter> coercer ) : base( activator, coercer )
		{}
	}

	/*public class ConventionActivateFactory<TResult> : ActivateFactory<ActivateParameter, TResult> where TResult : class
	{
		public ConventionActivateFactory()
		{}

		public ConventionActivateFactory( IActivator activator ) : base( activator )
		{}

		protected override Type DetermineType( ActivateParameter parameter )
		{
			var result = base.DetermineType( parameter ).Adapt().GetConventionCandidate();
			return result;
		}
	}*/
}