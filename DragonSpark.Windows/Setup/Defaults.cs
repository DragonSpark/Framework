using DragonSpark.Sources;
using DragonSpark.Sources.Parameterized;
using System;
using System.Configuration;
using System.IO;

namespace DragonSpark.Windows.Setup
{
	public static class Defaults
	{
		public static Func<FileInfo> UserSettingsPath { get; } = UserSettingsFile.Default.Fixed( ConfigurationUserLevel.PerUserRoamingAndLocal ).ToDelegate();
	}
}