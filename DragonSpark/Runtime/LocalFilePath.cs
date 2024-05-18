using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Alterations;
using System;
using System.IO;

namespace DragonSpark.Runtime;

sealed class LocalFilePath : Select<Uri, string>
{
	public static LocalFilePath Default { get; } = new();

	LocalFilePath() : base(x => x.LocalPath) {}
}

// TODO

public sealed class DirectoryUri : Alteration<string>
{
	public static DirectoryUri Default { get; } = new();

	DirectoryUri() : base(x => Path.GetDirectoryName(x)
	                               .Verify()
	                               .Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)) {}
}