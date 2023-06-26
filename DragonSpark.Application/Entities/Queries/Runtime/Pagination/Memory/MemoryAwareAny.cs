using DragonSpark.Application.Compose.Store.Operations;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Selection;

namespace DragonSpark.Application.Entities.Queries.Runtime.Pagination.Memory;

public class MemoryAwareAny<T> : Selecting<AnyInput<T>, bool>, IAny<T>
{
	protected MemoryAwareAny(IAny<T> previous, MemoryStoreProfile<AnyInput<T>> formatter)
		: base(previous.Then().Store().Using(formatter)) {}
}