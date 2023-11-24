using DragonSpark.Model.Results;
using System;

namespace DragonSpark.Reflection;

sealed class DeclaredValue<TAttribute, T> : Declared<TAttribute, T> where TAttribute : Attribute, IResult<T>
{
	public static DeclaredValue<TAttribute, T> Default { get; } = new();

	DeclaredValue() : base(Results<T>.Default.Get) {}
}