using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Runtime;

sealed class FilePathExists : Condition<string>
{
	public static FilePathExists Default { get; } = new();

	FilePathExists() : base(File.Exists) {}
}