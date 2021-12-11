using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace DragonSpark.Presentation.Browser;

public interface IHostedVariableDefinition<T> : IResulting<ProtectedBrowserStorageResult<T>>, IOperation<T>
{
	IOperation Remove { get; }
}