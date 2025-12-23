using DragonSpark.Model;
using System.Collections.Generic;

namespace DragonSpark.Application.AspNet.Entities.Queries.Runtime.Pagination;

public sealed class Page<T> : List<T>
{
	public static Page<T> Default { get; } = new();

	Page() : this(Empty.Array<T>(), null) {}

	public Page(IEnumerable<T> list, ulong? total) : base(list) => Total = total;

	public ulong? Total { get; }
}