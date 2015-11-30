using System;

namespace DragonSpark.Activation.FactoryModel
{
	public class ConstructFactory<TResult> : ActivationFactory<ConstructParameter, TResult> where TResult : class
	{
		public ConstructFactory() : this( SystemActivator.Instance )
		{}

		public ConstructFactory( IActivator activator ) : this( activator, new ConstructFactoryParameterQualifier<TResult>( activator ) )
		{}

		public ConstructFactory( IActivator activator, IFactoryParameterQualifier<ConstructParameter> qualifier ) : base( activator, qualifier )
		{}

		protected override TResult Activate( Type qualified, ConstructParameter parameter )
		{
			var result = Activator.Construct<TResult>( qualified, parameter.Arguments );
			return result;
		}
	}

	public class ActivateFactory<TParameter, TResult> : ActivationFactory<TParameter, TResult> where TResult : class where TParameter : ActivateParameter
	{
		public ActivateFactory()
		{}

		public ActivateFactory( IActivator activator ) : base( activator )
		{}

		protected override TResult Activate( Type qualified, TParameter parameter )
		{
			var result = Activator.Activate<TResult>( qualified, parameter.Name );
			return result;
		}
	}
}