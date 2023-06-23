using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Results;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace DragonSpark.Presentation.Environment.Browser;

public interface IClientVariable<T> : IResulting<ProtectedBrowserStorageResult<T>>, IOperation<T>
{
	IOperation Remove { get; }
}