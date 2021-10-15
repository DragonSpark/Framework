using DragonSpark.Application.Entities.Queries.Runtime.Materialize;
using DragonSpark.Model.Selection;
using Polly;

namespace DragonSpark.Application.Entities.Diagnostics;

sealed class PolicyAwareMaterialization<T> : ISelect<IAsyncPolicy, IMaterialization<T>>
{
	public static PolicyAwareMaterialization<T> Default { get; } = new PolicyAwareMaterialization<T>();

	PolicyAwareMaterialization() : this(DefaultMaterialization<T>.Default) {}

	readonly IMaterialization<T> _prototype;

	public PolicyAwareMaterialization(IMaterialization<T> prototype) => _prototype = prototype;

	public IMaterialization<T> Get(IAsyncPolicy parameter)
	{
		var any = new Any<T>(_prototype.Any.With(parameter));
		var counting = new Counting<T>(new Count<T>(_prototype.Counting.With(parameter)),
		                               new LargeCount<T>(_prototype.Counting.Large.With(parameter)));
		var materialize = new Sequences<T>(new ToList<T>(_prototype.Sequences.ToList.With(parameter)),
		                                   new ToArray<T>(_prototype.Sequences.ToArray.With(parameter)));
		var result = new Materialization<T>(any, counting, materialize);
		return result;
	}
}