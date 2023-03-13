using DragonSpark.Model.Selection;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace DragonSpark.Presentation.Environment.Browser;

public class KeyedClientVariables<T> : ISelect<ClientVariableKey, IClientVariable<T>>
{
	readonly ProtectedBrowserStorage _storage;

	protected KeyedClientVariables(ProtectedBrowserStorage storage) => _storage = storage;

	public IClientVariable<T> Get(ClientVariableKey parameter) => new ClientVariable<T>(parameter.ToString(), _storage);
}