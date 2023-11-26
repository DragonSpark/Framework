using Azure.Security.KeyVault.Secrets;
using DragonSpark.Model;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection;

namespace DragonSpark.Azure.Data;

public interface IKeyVaultVariableAccessor : ISelecting<string, KeyVaultSecret?>, IOperation<Pair<string, string>>
{
	IRemove Remove { get; }
}