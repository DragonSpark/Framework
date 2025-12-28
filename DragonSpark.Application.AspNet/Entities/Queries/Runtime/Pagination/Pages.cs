using DragonSpark.Contracts.Queries;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Queries.Runtime.Pagination;

public sealed class Pages<T> : StopAware<PageInput, PageResult<T>>, IPages<T>
{
	public Pages(Func<Stop<PageInput>, ValueTask<PageResult<T>>> select) : base(select) {}
}