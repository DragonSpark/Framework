using System;
using System.Collections.Generic;
using System.IO.Abstractions;

namespace DragonSpark.Testing.Framework.FileSystem
{
	/// <summary>
	/// Attribution: https://github.com/tathamoddie/System.IO.Abstractions
	/// </summary>
	[Serializable]
	public class MockDriveInfoFactory : IDriveInfoFactory
	{
		private readonly IFileSystemAccessor fileSystem;

		public MockDriveInfoFactory(IFileSystemAccessor fileSystem)
		{
			if (fileSystem == null)
			{
				throw new ArgumentNullException("fileSystem");
			}

			this.fileSystem = fileSystem;
		}

		public DriveInfoBase[] GetDrives()
		{
			var driveLetters = new HashSet<string>(new DriveEqualityComparer());
			foreach (var path in fileSystem.AllPaths)
			{
				var pathRoot = fileSystem.Path.GetPathRoot(path);
				driveLetters.Add(pathRoot);
			}

			var result = new List<DriveInfoBase>();
			foreach (string driveLetter in driveLetters)
			{
				try
				{
					var mockDriveInfo = new MockDriveInfo(fileSystem, driveLetter);
					result.Add(mockDriveInfo);
				}
				catch (ArgumentException)
				{
					// invalid drives should be ignored
				}
			}

			return result.ToArray();
		}

		private class DriveEqualityComparer : IEqualityComparer<string>
		{
			public bool Equals(string x, string y)
			{
				if (ReferenceEquals(x, y))
				{
					return true;
				}

				if (ReferenceEquals(x, null))
				{
					return false;
				}

				if (ReferenceEquals(y, null))
				{
					return false;
				}

				if (x[1] == ':' && y[1] == ':')
				{
					return char.ToUpperInvariant(x[0]) == char.ToUpperInvariant(y[0]);
				}

				return false;
			}

			public int GetHashCode(string obj)
			{
				return obj.ToUpperInvariant().GetHashCode();
			}
		}
	}
}
