using DragonSpark.Model;
using DragonSpark.Model.Commands;
using Microsoft.Data.SqlClient;

namespace DragonSpark.Application.Data;

public class RegisterEncryptedColumnKeyStore : FixedParameterCommand<Pair<string, SqlColumnEncryptionKeyStoreProvider>>
{
	protected RegisterEncryptedColumnKeyStore(string name, SqlColumnEncryptionKeyStoreProvider provider)
		: base(RegisterColumnEncryptionKeyStore.Default, Pairs.Create(name, provider)) {}
}