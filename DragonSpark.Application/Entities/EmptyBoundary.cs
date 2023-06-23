using DragonSpark.Model.Operations.Results;
using DragonSpark.Runtime;
using System;

namespace DragonSpark.Application.Entities;

sealed class EmptyBoundary : Instance<IDisposable>, IBoundary
{
	public static EmptyBoundary Default { get; } = new();

	EmptyBoundary() : base(EmptyDisposable.Default) {}
}