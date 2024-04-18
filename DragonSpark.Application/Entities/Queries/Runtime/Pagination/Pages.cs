using DragonSpark.Model.Operations.Selection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries.Runtime.Pagination;

public sealed class Pages<T> : Selecting<PageInput, Page<T>>, IPages<T>
{
	public Pages(Func<PageInput, ValueTask<Page<T>>> select) : base(select) {}
}