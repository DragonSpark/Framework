using DragonSpark.Application.AspNet.Entities.Initialization;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Search;

public sealed class InstallFullTextSearching : IInitializer
{
	public static InstallFullTextSearching Default { get; } = new();

	InstallFullTextSearching()
		: this("SELECT CAST(SERVERPROPERTY('IsFullTextInstalled') AS BIT)",
		       "IF NOT EXISTS (SELECT 1 FROM sys.fulltext_catalogs WHERE name = 'DefaultFullTextCatalog') CREATE FULLTEXT CATALOG DefaultFullTextCatalog AS DEFAULT;",
		       EnableFullTextCommands.Default) {}

	readonly string                                       _exists;
	readonly string                                       _install;
	readonly ISelect<IModel, IReadOnlyCollection<string>> _commands;

	public InstallFullTextSearching(string exists, string install,
	                                ISelect<IModel, IReadOnlyCollection<string>> commands)
	{
		_exists   = exists;
		_install  = install;
		_commands = commands;
	}

	public async ValueTask Get(Stop<DbContext> parameter)
	{
		var (subject, stop) = parameter;
		var supported = await subject.Database.SqlQueryRaw<bool?>(_exists, stop).FirstOrDefaultAsync().Off() ?? false;
		if (supported)
		{
			await subject.Database.ExecuteSqlRawAsync(_install, stop).Off();

			foreach (var command in _commands.Get(subject.Model))
			{
				await subject.Database.ExecuteSqlRawAsync(command, stop).Off();
			}
		}
	}
}