using DragonSpark.Sources.Parameterized;
using System;

namespace DragonSpark.Sources.Coercion
{
	public static class Extensions
	{
		public static ISource<TResult> Then<TParameter, TResult>( this ISource<TParameter> @this, IParameterizedSource<TParameter, TResult> coerce ) => @this.ToDelegate().Then( coerce.ToDelegate() );
		public static ISource<TResult> Then<TParameter, TResult>( this ISource<TParameter> @this, Func<TParameter, TResult> coerce ) => @this.ToDelegate().Then( coerce );
		public static ISource<TResult> Then<TParameter, TResult>( this Func<TParameter> @this, IParameterizedSource<TParameter, TResult> coerce ) => @this.Then( coerce.ToDelegate() );
		public static ISource<TResult> Then<TParameter, TResult>( this Func<TParameter> @this, Func<TParameter, TResult> coerce ) => coerce.WithParameter( @this );

		/*public static TResult GetCast<TParameter, TResult, TCast>( this IParameterizedSource<TParameter, TResult> @this, TCast parameter ) where TParameter : TCast => @this.Get( parameter.AsValid<TParameter>() );*/

		public static IParameterizedSource<TFrom, TResult> Accept<TFrom, TParameter, TResult>( this IParameterizedSource<TParameter, TResult> @this, IParameterizedSource<TFrom, TParameter> coerce ) => @this.ToDelegate().Accept( coerce.ToDelegate() );
		public static IParameterizedSource<TFrom, TResult> Accept<TFrom, TParameter, TResult>( this IParameterizedSource<TParameter, TResult> @this, Func<TFrom, TParameter> coerce ) => @this.ToDelegate().Accept( coerce );
		public static IParameterizedSource<TFrom, TResult> Accept<TFrom, TParameter, TResult>( this Func<TParameter, TResult> @this, IParameterizedSource<TFrom, TParameter> coerce ) => @this.Accept( coerce.ToDelegate() );
		public static IParameterizedSource<TFrom, TResult> Accept<TFrom, TParameter, TResult>( this Func<TParameter, TResult> @this, Func<TFrom, TParameter> coerce )
			=> new ParameterCoercionSource<TFrom, TParameter, TResult>( coerce, @this );

		public static IParameterizedSource<object, TTo> To<TResult, TTo>( this IParameterizedSource<object, TResult> @this ) => @this.ToDelegate().To<TResult, TTo>();
		public static IParameterizedSource<object, TTo> To<TResult, TTo>( this Func<object, TResult> @this ) => To( @this, CastCoercer<TResult, TTo>.Default );
		public static IParameterizedSource<TParameter, TTo> To<TParameter, TResult, TTo>( this IParameterizedSource<TParameter, TResult> @this, IParameterizedSource<TResult, TTo> coerce ) => @this.ToDelegate().To( coerce.ToDelegate() );
		public static IParameterizedSource<TParameter, TTo> To<TParameter, TResult, TTo>( this IParameterizedSource<TParameter, TResult> @this, Func<TResult, TTo> coerce ) => @this.ToDelegate().To( coerce );
		public static IParameterizedSource<TParameter, TTo> To<TParameter, TResult, TTo>( this Func<TParameter, TResult> @this, IParameterizedSource<TResult, TTo> coerce ) => @this.To( coerce.ToDelegate() );
		public static IParameterizedSource<TParameter, TTo> To<TParameter, TResult, TTo>( this Func<TParameter, TResult> @this, Func<TResult, TTo> coerce )
			=> new ResultCoercionSource<TParameter, TResult, TTo>( @this, coerce );
	}
}
