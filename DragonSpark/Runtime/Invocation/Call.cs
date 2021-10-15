using DragonSpark.Model.Selection;
using System;

namespace DragonSpark.Runtime.Invocation;

sealed class Call<T> : Select<Func<T>, T>
{
	public static Call<T> Default { get; } = new Call<T>();

	Call() : base(func => func()) {}
}