using DragonSpark.Model;
using DragonSpark.Model.Commands;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace DragonSpark.Application.Data;

sealed class RegisterColumnEncryptionKeyStore : IAssign<string, SqlColumnEncryptionKeyStoreProvider>
{
	public static RegisterColumnEncryptionKeyStore Default { get; } = new();

	RegisterColumnEncryptionKeyStore() : this(AssignColumnEncryptionKeyStoreProviders.Default.Execute) {}

	readonly Action<IDictionary<string, SqlColumnEncryptionKeyStoreProvider>> _assign;

	public RegisterColumnEncryptionKeyStore(Action<IDictionary<string, SqlColumnEncryptionKeyStoreProvider>> assign)
		=> _assign = assign;

	public void Execute(Pair<string, SqlColumnEncryptionKeyStoreProvider> parameter)
	{
		var (name, value) = parameter;

		var input = new Dictionary<string, SqlColumnEncryptionKeyStoreProvider>(1, StringComparer.OrdinalIgnoreCase)
		{
			{ name, value }
		};

		_assign(input);
	}
}