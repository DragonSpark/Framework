using DragonSpark.Model.Results;
using System.IO;
using System.Reflection;

namespace DragonSpark.Runtime.Environment;

public sealed class PrimaryDirectory : SelectedResult<Assembly, DirectoryInfo>
{
	public static PrimaryDirectory Default { get; } = new();

	PrimaryDirectory() : base(PrimaryAssembly.Default, AssemblyDirectory.Default) {}
}