using DragonSpark.Sources;
using DragonSpark.Sources.Parameterized;
using System;
using System.Collections.Immutable;

namespace DragonSpark.Configuration
{
	public static class Extensions
	{
		public static Func<object, ImmutableArray<IAlteration<T>>> Global<T>( this IItemSource<IAlteration<T>> @this, object _ ) => @this.Get;
		static ImmutableArray<IAlteration<T>> Get<T>( this IItemSource<IAlteration<T>> @this, object _ ) => @this.Get();
	}
}