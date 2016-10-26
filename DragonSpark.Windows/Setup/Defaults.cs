using DragonSpark.Sources;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Windows.FileSystem;
using System;

namespace DragonSpark.Windows.Setup
{
	public static class Defaults
	{
		public static Func<IFileInfo> UserSettingsPath { get; } = UserSettingsFile.Default.ToDelegate();
	}
}