using System;
using System.Linq;

namespace DragonSpark.Application.Entities.Queries.Runtime;

public readonly struct Query<T> : IDisposable
{
	readonly IDisposable _boundary;

	public Query(IQueryable<T> subject, IDisposable boundary)
	{
		Subject   = subject;
		_boundary = boundary;
	}

	public IQueryable<T> Subject { get; }

	public void Dispose()
	{
		_boundary.Dispose();
	}
}