using DragonSpark.Expressions;
using DragonSpark.Extensions;
using DragonSpark.Sources;
using DragonSpark.Sources.Parameterized;
using System;
using System.Collections.Immutable;

namespace DragonSpark.TypeSystem.Generics
{
	public class GenericInvocationFactory<TParameter, TResult> : ParameterizedSourceBase<TParameter, TResult> where TParameter : class
	{
		readonly Func<Type, Func<TParameter, TResult>> get;

		public GenericInvocationFactory( Type genericTypeDefinition, Type owningType, string methodName ) 
			: this( new DelegateSource( owningType.Adapt().GenericFactoryMethods[ methodName ], genericTypeDefinition ).ToSourceDelegate().Cache() ) {}

		GenericInvocationFactory( Func<Type, Func<TParameter, TResult>> get )
		{
			this.get = get;
		}

		sealed class DelegateSource : ParameterizedSourceBase<Type, Func<TParameter, TResult>>
		{
			readonly IGenericMethodContext<Invoke> context;
			readonly Type genericType;

			public DelegateSource( IGenericMethodContext<Invoke> context, Type genericType )
			{
				this.context = context;
				this.genericType = genericType;
			}

			public override Func<TParameter, TResult> Get( Type parameter ) => 
				context.Make( parameter.Adapt().GetTypeArgumentsFor( genericType ) ).Get( new[] { parameter }.ToImmutableArray() ).Invoke<TParameter, TResult>;
		}

		public override TResult Get( TParameter parameter ) => get( parameter.GetType() )( parameter );
	}
}