using DragonSpark.Model.Commands;
using System.IO;

namespace DragonSpark.Runtime;

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

