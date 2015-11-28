using System.Linq;
using DragonSpark.Extensions;

namespace DragonSpark.Activation
{
	public interface IFactoryParameterQualifier<out TParameter>
	{
		TParameter Qualify( object context );
	}

	public class FactoryParameterQualifier<TParameter> : IFactoryParameterQualifier<TParameter>
	{
		public static FactoryParameterQualifier<TParameter> Instance { get; } = new FactoryParameterQualifier<TParameter>();

		public TParameter Qualify( object context )
		{
			var result = context is TParameter ? (TParameter)context : context.Transform( Construct );
			return result;
		}

		protected virtual TParameter Construct( object parameter )
		{
			var result = (TParameter)typeof(TParameter).Extend().FindConstructor( parameter.GetType() ).Transform( info => info.Invoke( new[] { info.GetParameters().First().ParameterType.Extend().Qualify( parameter ) } ) );
			return result;
		}
	}

	public abstract class FactoryBase<TParameter, TResult> : IFactory<TParameter, TResult> where TResult : class
	{
		readonly IFactoryParameterQualifier<TParameter> qualifier;

		protected FactoryBase() : this( FactoryParameterQualifier<TParameter>.Instance )
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