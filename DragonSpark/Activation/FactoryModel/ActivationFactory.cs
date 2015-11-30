using System;
using DragonSpark.Extensions;

namespace DragonSpark.Activation.FactoryModel
{
	public abstract class ActivationFactory<TParameter, TResult> : FactoryBase<TParameter, TResult> where TParameter : ActivationParameter where TResult : class
	{
		protected ActivationFactory() : this( SystemActivator.Instance )
		{}

		protected ActivationFactory( IActivator activator ) : this( activator, new FactoryParameterQualifier<TParameter>( activator ) )
		{}

		protected ActivationFactory( IActivator activator, IFactoryParameterQualifier<TParameter> qualifier ) : base( qualifier )
		{
			Activator = activator;
		}

		protected IActivator Activator { get; }

		protected override TResult CreateItem( TParameter parameter )
		{
			var qualified = DetermineType( parameter ).With( extension => extension.Extend().GuardAsAssignable<TResult>( nameof(parameter) ) );
			var result = qualified.With( type => Activate( type, parameter ) );
			return result;
		}

		protected virtual Type DetermineType( TParameter parameter )
		{
			return parameter.Type;
		}

		protected abstract TResult Activate( Type qualified, TParameter parameter );

		/*protected override TParameter DetermineParameter( object parameter )
		{
			var result = parameter.AsTo<Type, TParameter>( type => (TParameter)type ) ?? base.DetermineParameter( parameter );
			/*var asdf = parameter.Transform( o => typeof(TParameter).Extend().IsAssignableFrom( o.GetType() ) );
			var qualified = typeof(TParameter).Extend().IsInstanceOfType( parameter ) ? (TParameter)parameter : default(TParameter);#1#

			return result;
		}*/
	}
}