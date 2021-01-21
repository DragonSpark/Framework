using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace DragonSpark.Presentation.Components.State
{
	public interface ISessionVariable<T> : ISelecting<string, ProtectedBrowserStorageResult<T>>,
	                                       IOperation<(string Key, T Value)>
	{
		IRemove Remove { get; }
	}
}