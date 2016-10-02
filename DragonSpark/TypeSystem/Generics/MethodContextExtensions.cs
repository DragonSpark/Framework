using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using DragonSpark.Application;
using DragonSpark.Expressions;
using DragonSpark.Sources.Parameterized;

namespace DragonSpark.TypeSystem.Generics
{
	public static class MethodContextExtensions
	{
		readonly static Func<IEnumerable<object>, ImmutableArray<Type>> ToType = ObjectTypeFactory.Default.ToSourceDelegate();

		public static TResult Invoke<TParameter, TResult>( this Invoke @this, TParameter argument ) => (TResult)@this.Invoke( argument );

		public static T Invoke<T>( this MethodContext<Invoke> @this ) => Invoke<T>( @this, Items<object>.Default );

		public static T Invoke<T>( this MethodContext<Invoke> @this, params object[] arguments ) => (T)@this.Get( ToType( arguments ) ).Invoke( arguments );

		public static void Invoke( this MethodContext<Execute> @this, params object[] arguments ) => @this.Get( ToType( arguments ) ).Invoke( arguments );
	}
}