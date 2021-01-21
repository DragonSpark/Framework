using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace DragonSpark.Presentation.Components.State
{
	public interface ISessionVariableDefinition<T> : IResulting<ProtectedBrowserStorageResult<T>>, IOperation<T>
	{
		IOperation Remove { get; }
	}
}