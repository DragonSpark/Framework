using PostSharp.Patterns.Contracts;

namespace DragonSpark.Activation.FactoryModel
{
	public abstract class FactoryBase<TParameter, TResult> : IFactory<TParameter, TResult>
	{
		readonly IFactoryParameterCoercer<TParameter> coercer;

		protected FactoryBase() : this( FixedFactoryParameterCoercer<TParameter>.Instance )
		{}

		protected FactoryBase( [Required]IFactoryParameterCoercer<TParameter> coercer )
		{
			this.coercer = coercer;
		}

		protected abstract TResult CreateItem( [Required]TParameter parameter );

		public TResult Create( TParameter parameter )
		{
			var result = CreateItem( parameter );
			return result;
		}

		object IFactoryWithParameter.Create( object parameter )
		{
			var qualified = coercer.Coerce( parameter );
			var result = Create( qualified );
			return result;
		}
	}

	public abstract class FactoryBase<TResult> : IFactory<TResult>
	{
		protected abstract TResult CreateItem();

		public TResult Create() => CreateItem();

		object IFactory.Create() => Create();
	}
}