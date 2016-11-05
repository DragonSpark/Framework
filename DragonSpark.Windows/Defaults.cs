using System;
using DragonSpark.Sources;
using DragonSpark.Windows.FileSystem;

namespace DragonSpark.Windows
{
	public static class Defaults
	{
		public static Func<IFileInfo> UserSettingsPath { get; } = UserSettingsFile.Default.ToDelegate();
	}
}