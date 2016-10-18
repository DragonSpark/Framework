﻿using System;
using System.Text.RegularExpressions;

namespace DragonSpark.Testing.Framework.FileSystem
{
	/// <summary>
	/// Attribution: https://github.com/tathamoddie/System.IO.Abstractions
	/// </summary>
	internal static class MockUnixSupport
    {
        internal static string Path(string path, Func<bool> isUnixF = null)
        {
            var isUnix = isUnixF ?? IsUnixPlatform;

            if (isUnix())
            {
                path = Regex.Replace(path, @"^[a-zA-Z]:(?<path>.*)$", "${path}");
                path = path.Replace(@"\", "/");
            }

            return path;
        }

        internal static string Separator(Func<bool> isUnixF = null)
        {
            var isUnix = isUnixF ?? IsUnixPlatform;
            return isUnix() ? "/" : @"\";
        }

        internal static bool IsUnixPlatform()
        {
            int p = (int)Environment.OSVersion.Platform;
            return (p == 4) || (p == 6) || (p == 128);
        }
    }
}
