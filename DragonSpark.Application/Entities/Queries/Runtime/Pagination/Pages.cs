using DragonSpark.Model.Operations.Selection;
using DragonSpark.Model.Selection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries.Runtime.Pagination;

public class Pages<T> : Selecting<PageInput, Page<T>>, IPages<T>
{
	public Pages(ISelect<PageInput, ValueTask<Page<T>>> select) : base(select) {}

	public Pages(Func<PageInput, ValueTask<Page<T>>> select) : base(select) {}
}