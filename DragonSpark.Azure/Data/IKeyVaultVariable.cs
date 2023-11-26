using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Results;

namespace DragonSpark.Azure.Data;

public interface IKeyVaultVariable : IResulting<string?>, IOperation<string>
{
	IOperation Remove { get; }
}