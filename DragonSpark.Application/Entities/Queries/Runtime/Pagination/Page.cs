using DragonSpark.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DragonSpark.Application.Entities.Queries.Runtime.Pagination;

public sealed class Page<T> : Collection<T>
{
	public static Page<T> Default { get; } = new Page<T>();

	Page() : this(Empty.Array<T>(), null) {}

	public Page(IList<T> list, ulong? total) : base(list) => Total = total;

	public ulong? Total { get; }
}