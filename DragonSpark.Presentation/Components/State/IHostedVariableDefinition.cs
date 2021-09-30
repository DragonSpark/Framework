using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace DragonSpark.Presentation.Components.State
{
	public interface IHostedVariableDefinition<T> : IResulting<ProtectedBrowserStorageResult<T>>, IOperation<T>
	{
		IOperation Remove { get; }
	}
}