using NetFabric.Hyperlinq;
using System;

namespace DragonSpark.Application.Model.Sequences;

public readonly record struct Actions<TView, TModel>(Lease<TView> Add, Lease<Update<TView, TModel>> Update,
                                                     Lease<TModel> Delete) : IDisposable
{
	public bool Any() => Add.Length > 0 || Update.Length > 0 || Delete.Length > 0;

	public ActionSpans<TView, TModel> AsSpans() => new(Add.Memory, Update.Memory, Delete.Memory);

	public ActionMemory<TView, TModel> AsMemory() => new(Add.Memory, Update.Memory, Delete.Memory);

	public void Dispose()
	{
		Add.Dispose();
		Update.Dispose();
		Delete.Dispose();
	}
}