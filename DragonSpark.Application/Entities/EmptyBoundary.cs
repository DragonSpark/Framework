using DragonSpark.Runtime;
using System;

namespace DragonSpark.Application.Entities;

sealed class EmptyBoundary : DragonSpark.Model.Operations.Instance<IDisposable>, IBoundary
{
	public static EmptyBoundary Default { get; } = new();

	EmptyBoundary() : base(EmptyDisposable.Default) {}
}