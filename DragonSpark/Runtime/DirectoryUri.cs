using DragonSpark.Compose;
using DragonSpark.Model.Selection.Alterations;
using System.IO;

namespace DragonSpark.Runtime;

public sealed class DirectoryUri : Alteration<string>
{
	public static DirectoryUri Default { get; } = new();

	DirectoryUri()
		: base(x => Path.GetDirectoryName(x)
		                .Verify()
		                .Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)) {}
}