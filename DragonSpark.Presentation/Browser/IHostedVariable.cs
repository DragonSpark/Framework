using DragonSpark.Model.Operations;
using DragonSpark.Presentation.Components.State;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace DragonSpark.Presentation.Browser;

public interface IHostedVariable<T> : ISelecting<string, ProtectedBrowserStorageResult<T>>,
                                      IOperation<(string Key, T Value)>
{
	IRemove Remove { get; }
}