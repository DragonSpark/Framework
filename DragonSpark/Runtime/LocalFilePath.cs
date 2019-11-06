using System;
using DragonSpark.Model.Selection;

namespace DragonSpark.Runtime
{
	sealed class LocalFilePath : Select<Uri, string>
	{
		public static LocalFilePath Default { get; } = new LocalFilePath();

		LocalFilePath() : base(x => x.LocalPath) {}
	}
}