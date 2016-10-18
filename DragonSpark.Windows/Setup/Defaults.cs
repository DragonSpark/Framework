using DragonSpark.Sources;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Windows.FileSystem;
using System;
using System.Configuration;

namespace DragonSpark.Windows.Setup
{
	public static class Defaults
	{
		public static Func<IFileInfo> UserSettingsPath { get; } = UserSettingsFile.Default.Fixed( ConfigurationUserLevel.PerUserRoamingAndLocal ).ToDelegate();
	}
}