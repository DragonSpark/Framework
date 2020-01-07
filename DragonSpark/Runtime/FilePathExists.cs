using DragonSpark.Model.Selection.Conditions;
using System.IO;

namespace DragonSpark.Runtime
{
	sealed class FilePathExists : Condition<string>
	{
		public static FilePathExists Default { get; } = new FilePathExists();

		FilePathExists() : base(File.Exists) {}
	}
}