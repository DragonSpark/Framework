using System;
using System.ComponentModel.DataAnnotations;

namespace DragonSpark.Activation.FactoryModel
{
	public abstract class ActivationFactory<TParameter, TResult> : FactoryBase<TParameter, TResult> where TParameter : ActivationParameter where TResult : class
	{
		readonly Activator.Get activator;

		protected ActivationFactory( Activator.Get activator ) : this( activator, new FactoryParameterCoercer<TParameter>( activator ) )
		{}

		protected ActivationFactory( [Required]Activator.Get activator, IFactoryParameterCoercer<TParameter> coercer ) : base( coercer )
		{
			this.activator = activator;
		}

		protected IActivator Activator => activator();

		protected override TResult CreateItem( TParameter parameter ) => Activate( parameter );

		protected abstract TResult Activate( TParameter parameter );
	}
}