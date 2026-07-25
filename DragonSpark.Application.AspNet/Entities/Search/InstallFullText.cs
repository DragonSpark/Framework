using DragonSpark.Compose;
using DragonSpark.Contracts.Model;
using DragonSpark.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Application.AspNet.Entities.Search;

sealed class InstallFullText : IFormatter<InstallFullTextInput>
{
	public static InstallFullText Default { get; } = new();

	InstallFullText() : this(DefaultCatalogName.Default) {}

	readonly string _name;

	public InstallFullText(string name) => _name = name;

	public string Get(InstallFullTextInput parameter)
	{
		var (type, table) = parameter;
		var schema     = type.GetSchema() ?? "dbo";
		var identifier = StoreObjectIdentifier.Table(table, schema);
		var key        = type.FindPrimaryKey()?.GetName(identifier);
		var names      = new List<string>();
		foreach (var property in type.ClrType.GetProperties()
		                             .Where(p => p.Attribute<FullTextAttribute>() is not null))
		{
			var columnName = type.FindProperty(property.Name)?.GetColumnName(identifier);
			if (columnName is not null)
			{
				names.Add($"[{columnName}]");
			}
		}

		return names.Any() && key.IsAssigned()
			       ? $"""
			          IF NOT EXISTS (SELECT 1 FROM sys.fulltext_indexes fi JOIN sys.objects o ON fi.object_id = o.object_id WHERE o.name = '{table}')
			          BEGIN
			            CREATE FULLTEXT INDEX ON [{schema}].[{table}] ({string.Join(", ", names)}) KEY INDEX [{key}] ON {_name};
			          END
			          """
			       : string.Empty;
	}
}