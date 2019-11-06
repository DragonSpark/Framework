using System.IO;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Io
{
	sealed class FilePathExists : Condition<string>
	{
		public static FilePathExists Default { get; } = new FilePathExists();

		FilePathExists() : base(File.Exists) {}
	}
}