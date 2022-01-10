using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using System.IO;
using System.Reflection;

namespace DragonSpark.Runtime.Environment;

public sealed class AssemblyDirectory : ISelect<Assembly, DirectoryInfo>
{
	public static AssemblyDirectory Default { get; } = new();

	AssemblyDirectory() {}

	public DirectoryInfo Get(Assembly parameter) => new(Path.GetDirectoryName(parameter.Location).Verify());
}