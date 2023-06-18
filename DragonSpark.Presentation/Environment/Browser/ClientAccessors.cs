using DragonSpark.Model.Selection;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace DragonSpark.Presentation.Environment.Browser;

sealed class ClientAccessors<T> : ISelect<ProtectedBrowserStorage, IClientVariableAccessor<T>>
{
	public static ClientAccessors<T> Default { get; } = new();

	ClientAccessors() {}

	public IClientVariableAccessor<T> Get(ProtectedBrowserStorage parameter)
	{
		var accessor = new ClientVariableAccessor<T>(parameter);
		var previous = new ConnectionAwareClientVariableAccessor<T>(accessor);
		var result   = new CryptographicKeyAwareClientVariableAccessor<T>(previous);
		return result;
	}
}