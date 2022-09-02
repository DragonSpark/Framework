using DragonSpark.Model.Commands;
using System.IO;

namespace DragonSpark.Runtime;

/// <summary>
///  ATTRIBUTION: https://github.com/libgit2/libgit2sharp/issues/1354#issuecomment-277936895
/// </summary>
public sealed class DeleteDirectoryRecursively : ICommand<DirectoryInfo>
{
	public static DeleteDirectoryRecursively Default { get; } = new();

	DeleteDirectoryRecursively() {}

	public void Execute(DirectoryInfo parameter)
	{
		foreach (var directory in parameter.EnumerateDirectories())
		{
			Execute(directory);
		}

		foreach (var file in parameter.EnumerateFiles())
		{
			file.Attributes = FileAttributes.Normal;
			file.Delete();
		}

		parameter.Delete(true);
	}
}

