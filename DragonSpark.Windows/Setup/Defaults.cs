using DragonSpark.Sources;
using DragonSpark.Sources.Parameterized;
using System;
using System.Configuration;
using System.IO.Abstractions;

namespace DragonSpark.Windows.Setup
{
	public static class Defaults
	{
		public static Func<FileInfoBase> UserSettingsPath { get; } = UserSettingsFile.Default.Fixed( ConfigurationUserLevel.PerUserRoamingAndLocal ).ToDelegate();
	}
}