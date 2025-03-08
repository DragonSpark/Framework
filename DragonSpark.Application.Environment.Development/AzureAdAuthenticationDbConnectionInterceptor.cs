using System;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Identity;
using DragonSpark.Compose;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DragonSpark.Application.Environment.Development;

// ATTRIBUTION: https://devblogs.microsoft.com/azure-sdk/azure-identity-with-sql-graph-ef/
sealed class AzureAdAuthenticationDbConnectionInterceptor : DbConnectionInterceptor
{
	public static AzureAdAuthenticationDbConnectionInterceptor Default { get; } = new();

	AzureAdAuthenticationDbConnectionInterceptor()
		: this(new DefaultAzureCredential(),
		       new TokenRequestContext(new[] { "https://database.windows.net//.default" })) {}

	readonly TokenCredential     _credential;
	readonly TokenRequestContext _context;

	public AzureAdAuthenticationDbConnectionInterceptor(TokenCredential credential, TokenRequestContext context)
	{
		_credential = credential;
		_context    = context;
	}

	public override InterceptionResult ConnectionOpening(
		DbConnection connection,
		ConnectionEventData eventData,
		InterceptionResult result)
	{
		var sqlConnection = (SqlConnection)connection;
		if (Access(sqlConnection))
		{
			sqlConnection.AccessToken = _credential.GetToken(_context, default).Token;
		}

		return base.ConnectionOpening(connection, eventData, result);
	}

	// ReSharper disable once TooManyArguments
	public override async ValueTask<InterceptionResult> ConnectionOpeningAsync(
		DbConnection connection,
		ConnectionEventData eventData,
		InterceptionResult result,
		CancellationToken cancellationToken = default)
	{
		var sql = (SqlConnection)connection;
		if (Access(sql))
		{
			var token = await _credential.GetTokenAsync(_context, cancellationToken).Off();
			sql.AccessToken = token.Token;
		}

		return await base.ConnectionOpeningAsync(connection, eventData, result, cancellationToken).Off();
	}

	static bool Access(SqlConnection connection)
	{
		var builder = new SqlConnectionStringBuilder(connection.ConnectionString);
		return builder.DataSource.Contains("database.windows.net", StringComparison.OrdinalIgnoreCase) &&
		       string.IsNullOrEmpty(builder.UserID);
	}
}
