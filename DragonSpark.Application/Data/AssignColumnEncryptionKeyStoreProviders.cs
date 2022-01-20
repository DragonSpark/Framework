using DragonSpark.Model.Commands;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace DragonSpark.Application.Data;

sealed class AssignColumnEncryptionKeyStoreProviders : Command<IDictionary<string, SqlColumnEncryptionKeyStoreProvider>>
{
	public static AssignColumnEncryptionKeyStoreProviders Default { get; } = new();

	AssignColumnEncryptionKeyStoreProviders() : base(SqlConnection.RegisterColumnEncryptionKeyStoreProviders) {}
}