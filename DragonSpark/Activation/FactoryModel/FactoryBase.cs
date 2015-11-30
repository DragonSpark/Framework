namespace DragonSpark.Activation.FactoryModel
{
	public abstract class FactoryBase<TParameter, TResult> : IFactory<TParameter, TResult> where TResult : class
	{
		readonly IFactoryParameterQualifier<TParameter> qualifier;

		protected FactoryBase() : this( new FactoryParameterQualifier<TParameter>() )
		{}

		protected FactoryBase( IFactoryParameterQualifier<TParameter> qualifier )
		{
			this.qualifier = qualifier;
		}

		protected abstract TResult CreateItem( TParameter parameter );

		public TResult Create( TParameter parameter )
		{
			var result = CreateItem( parameter );
			return result;
		}

		object IFactoryWithParameter.Create( object parameter )
		{
			var qualified = qualifier.Qualify( parameter );
			var result = Create( qualified );
			return result;
		}
	}

	public abstract class FactoryBase<TResult> : IFactory<TResult> where TResult : class
	{
		protected abstract TResult CreateItem();

		public TResult Create()
		{
			return CreateItem();
		}

		object IFactory.Create()
		{
			var result = CreateItem();
			return result;
		}
	}
}