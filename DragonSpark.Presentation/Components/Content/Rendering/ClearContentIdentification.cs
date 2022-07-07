using DragonSpark.Compose;
using DragonSpark.Model;

namespace DragonSpark.Presentation.Components.Content.Rendering;

sealed class ClearContentIdentification : IClearContentIdentification
{
	readonly ContentIdentifierStore _store;

	public ClearContentIdentification(ContentIdentifierStore store) => _store = store;

	public void Execute(None parameter)
	{
		_store.Execute();
	}
}