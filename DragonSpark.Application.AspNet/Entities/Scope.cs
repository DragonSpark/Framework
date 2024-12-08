using Microsoft.EntityFrameworkCore;
using System;

namespace DragonSpark.Application.AspNet.Entities;

public readonly record struct Scope(DbContext Owner, IDisposable Disposable) : IDisposable
{
	public Scope(DbContext Owner) : this(Owner, Owner) {}

	public void Dispose()
	{
		Disposable.Dispose();
	}
}