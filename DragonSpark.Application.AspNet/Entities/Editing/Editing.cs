using DragonSpark.Application.AspNet.Entities.Queries.Composition;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Model.Selection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Editing;

public class Editing<TIn, T> : StopAware<TIn, Edit<T>>, IEdit<TIn, T>
{
	protected Editing(IScopes scope, IQuery<TIn, T> query) : base(scope.Then().Use(query).Edit.Single()) {}

	protected Editing(ISelect<Stop<TIn>, ValueTask<Edit<T>>> select) : base(select) {}

	protected Editing(Func<Stop<TIn>, ValueTask<Edit<T>>> select) : base(select) {}
}