using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences.Memory;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Hosting;
using NetFabric.Hyperlinq;
using System.Buffers;
using System.IO;
using System.Linq;

namespace DragonSpark.Application.Configuration;

sealed class SharedSettingsSources : ISelect<IHostEnvironment, Leasing<JsonConfigurationSource>>
{
	public static SharedSettingsSources Default { get; } = new();

	SharedSettingsSources() : this(SharedSettingsRoot.Default) {}

	readonly ISelect<IHostEnvironment, DirectoryInfo> _root;

	public SharedSettingsSources(ISelect<IHostEnvironment, DirectoryInfo> root) => _root = root;

	public Leasing<JsonConfigurationSource> Get(IHostEnvironment parameter)
	{
		var directory = _root.Get(parameter);
		var result = directory.Exists
			             ? directory.GetFiles("*.json", SearchOption.AllDirectories)
			                        .OrderBy(x => x.FullName.Count(y => y == Path.DirectorySeparatorChar))
			                        .ThenBy(x => x.Name.Count(y => y == '.'))
			                        .AsEnumerable()
			                        .AsValueEnumerable()
			                        .Select(x => new JsonConfigurationSource
			                        {
				                        Path           = x.FullName,
				                        ReloadOnChange = true
			                        })
			                        .ToArray(ArrayPool<JsonConfigurationSource>.Shared)
			                        .Then()
			             : Leasing<JsonConfigurationSource>.Default;
		return result;
	}
}