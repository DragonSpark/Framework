using DragonSpark.TypeSystem;
using System;
using System.Linq.Expressions;

namespace DragonSpark.Expressions
{
	public abstract class CompiledDelegateFactoryBase<TParameter, TResult> //: ParameterizedSourceBase<TParameter, TResult>
	{
		readonly ParameterExpression parameterExpression;
		readonly Func<ExpressionBodyParameter<TParameter>, Expression> bodySource;

		readonly Type convertType;

		protected CompiledDelegateFactoryBase( Func<ExpressionBodyParameter<TParameter>, Expression> bodySource ) : this( Parameter.Default, bodySource ) {}

		protected CompiledDelegateFactoryBase( ParameterExpression parameterExpression, Func<ExpressionBodyParameter<TParameter>, Expression> bodySource )
		{
			this.parameterExpression = parameterExpression;
			this.bodySource = bodySource;

			var type = Defaults<TResult>.Info.GetDeclaredMethod( nameof(Invoke) ).ReturnType;
			convertType = type != Defaults.Void && type != Defaults<TResult>.Type ? type : null;
		}

		public virtual TResult Get( TParameter parameter )
		{
			var body = bodySource( new ExpressionBodyParameter<TParameter>( parameter, parameterExpression ) );
			var converted = convertType != null ? Expression.Convert( body, convertType ) : body;
			var result = Expression.Lambda<TResult>( converted, parameterExpression ).Compile();
			return result;
		}
	}
}