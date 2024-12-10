using System.Buffers;
using System.IO;
using System.Linq;
using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences.Memory;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Hosting;
using NetFabric.Hyperlinq;

namespace DragonSpark.Application.Configuration;

sealed class AmbientConfigurationSources : ISelect<IHostEnvironment, Leasing<JsonConfigurationSource>>
{
    public static AmbientConfigurationSources Default { get; } = new();

    AmbientConfigurationSources() : this(AmbientConfigurationRoot.Default) { }

    readonly ISelect<IHostEnvironment, DirectoryInfo> _root;

    public AmbientConfigurationSources(ISelect<IHostEnvironment, DirectoryInfo> root) => _root = root;

    [MustDisposeResource]
    public Leasing<JsonConfigurationSource> Get(IHostEnvironment parameter)
    {
        var directory = _root.Get(parameter);
        if (directory.Exists)
        {
            var first = directory.EnumerateFiles($"appsettings.{parameter.EnvironmentName}.json",
                                                 SearchOption.AllDirectories);
            var second = parameter.IsDevelopment()
                             ? directory.EnumerateFiles($"appsettings.{parameter.EnvironmentName}.developer.json",
                                                        SearchOption.AllDirectories)
                             : Empty.Array<FileInfo>();

            var others = first.Union(second);
            return directory.EnumerateFiles("appsettings.json", SearchOption.AllDirectories)
                            .Concat(others)
                            .OrderBy(x => x.FullName.Count(y => y == Path.DirectorySeparatorChar))
                            .ThenBy(x => x.Name.Count(y => y == '.'))
                            .AsValueEnumerable()
                            .Select(x => new JsonConfigurationSource
                            {
                                Path = x.FullName,
                                ReloadOnChange = true
                            })
                            .ToArray(ArrayPool<JsonConfigurationSource>.Shared)
                            .Then();
        }

        return Leasing<JsonConfigurationSource>.Default;
    }
}
