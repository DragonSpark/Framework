using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Entities;

[method: MustDisposeResource]
public readonly record struct Scope(DbContext Owner, IDisposable Disposable) : IDisposable
{
	[MustDisposeResource]
	public Scope(DbContext Owner) : this(Owner, Owner) {}

	public void Dispose()
	{
		Disposable.Dispose();
	}
}
