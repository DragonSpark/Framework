using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DragonSpark.Application.AspNet.Entities.Search;

sealed class EnableFullTextCommands : ISelect<IModel, IReadOnlyCollection<string>>
{
	public static EnableFullTextCommands Default { get; } = new();

	EnableFullTextCommands() : this(InstallFullText.Default) {}

	readonly IFormatter<InstallFullTextInput> _install;

	public EnableFullTextCommands(IFormatter<InstallFullTextInput> install) => _install = install;

	public IReadOnlyCollection<string> Get(IModel parameter)
	{
		var result = new List<string>();

		foreach (var type in parameter.GetEntityTypes())
		{
			var table = type.GetTableName();
			if (table.IsAssigned())
			{
				var install = _install.Get(new(type, table));
				if (install.IsAssigned())
				{
					result.Add(install);
				}
			}
		}

		return result;
	}
}