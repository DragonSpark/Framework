using DragonSpark.Model.Selection;
using System;

namespace DragonSpark.Runtime.Invocation;

sealed class PostAccessToken<T> : Select<Func<T>, T>
{
	public static PostAccessToken<T> Default { get; } = new();

	PostAccessToken() : base(func => func()) {}
}