using DragonSpark.Extensions;
using DragonSpark.Runtime;
using DragonSpark.Sources;
using DragonSpark.TypeSystem;
using System;
using System.Collections.Generic;

namespace DragonSpark.Aspects.Build
{
	// ReSharper disable once PossibleInfiniteInheritance

	// ReSharper disable once PossibleInfiniteInheritance
	public interface ITypedPairs<TParameter, TResult> : IEnumerable<ValueTuple<TypeAdapter, Func<TParameter, TResult>>> {}

	// ReSharper disable once PossibleInfiniteInheritance
	public class TypedPairs<T> : TypedPairs<object, T>, ITypedPairs<T>
	{
		public TypedPairs( IEnumerable<ValueTuple<TypeAdapter, Func<object, T>>> items ) : this( items.Fixed() ) {}
		public TypedPairs( params ValueTuple<TypeAdapter, Func<object, T>>[] items ) : base( items ) {}
	}

	// ReSharper disable once PossibleInfiniteInheritance
	public class TypedPairs<TParameter, TResult> : ItemSource<ValueTuple<TypeAdapter, Func<TParameter, TResult>>>
	{
		public TypedPairs( params ValueTuple<TypeAdapter, Func<TParameter, TResult>>[] items ) : base( items ) {}
	}
}
