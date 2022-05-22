using DragonSpark.Model;
using DragonSpark.Model.Operations;
using DragonSpark.Presentation.Components.State;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace DragonSpark.Presentation.Environment.Browser;

public interface IClientVariableAccessor<T> : ISelecting<string, ProtectedBrowserStorageResult<T>>, IOperation<Pair<string, T>>
{
	IRemove Remove { get; }
}