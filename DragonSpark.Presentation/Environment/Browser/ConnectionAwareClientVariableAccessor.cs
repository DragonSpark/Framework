using DragonSpark.Model;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection;
using DragonSpark.Presentation.Components.State;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Environment.Browser;

public sealed class ConnectionAwareClientVariableAccessor<T> : IClientVariableAccessor<T>
{
	readonly ISelecting<string, ProtectedBrowserStorageResult<T>> _get;
	readonly IOperation<Pair<string, T>>                          _set;

	public ConnectionAwareClientVariableAccessor(IClientVariableAccessor<T> previous)
		: this(previous, new ConnectionAware<Pair<string, T>>(previous), previous.Remove) {}

	public ConnectionAwareClientVariableAccessor(ISelecting<string, ProtectedBrowserStorageResult<T>> get,
	                                             IOperation<Pair<string, T>> set, IRemove remove)
	{
		Remove = remove;
		_get   = get;
		_set   = set;
	}

	public IRemove Remove { get; }

	public ValueTask<ProtectedBrowserStorageResult<T>> Get(string parameter) => _get.Get(parameter);

	public ValueTask Get(Pair<string, T> parameter) => _set.Get(parameter);
}