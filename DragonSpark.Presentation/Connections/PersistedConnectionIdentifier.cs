using DragonSpark.Model.Results;
using System;

namespace DragonSpark.Presentation.Connections;

sealed class PersistedConnectionIdentifier : IResult<Guid?>
{
	readonly ConnectionIdentifierStore _store;

	public PersistedConnectionIdentifier(ConnectionIdentifierStore store) => _store   = store;

	public Guid? Get()
	{
		var store = _store.Get();
		return store.Success ? store.Value : null;
	}
}