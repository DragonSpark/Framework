using System;
using System.IO;
using System.IO.Abstractions;

namespace DragonSpark.Testing.Framework.FileSystem
{
	/// <summary>
	/// Attribution: https://github.com/tathamoddie/System.IO.Abstractions
	/// </summary>
	[Serializable]
	public class MockDriveInfo : DriveInfoBase
	{
		readonly IFileSystem fileSystem;

		public MockDriveInfo(IFileSystem fileSystem, string name)
		{
			const string driveSeparator = @":\";
			if (name.Length == 1)
			{
				name = char.ToUpperInvariant(name[0]) + driveSeparator;
			}
			else if (name.Length == 2 && name[1] == ':')
			{
				name = char.ToUpperInvariant(name[0]) + driveSeparator;
			}
			else if (name.Length == 3 && name.EndsWith(driveSeparator, StringComparison.Ordinal))
			{
				name = char.ToUpperInvariant(name[0]) + driveSeparator;
			}
			else
			{
				MockPath.CheckInvalidPathChars(name);
				name = fileSystem.Path.GetPathRoot(name);

				if (string.IsNullOrEmpty(name) || name.StartsWith(@"\\", StringComparison.Ordinal))
				{
					throw new ArgumentException(
						@"Object must be a root directory (""C:\"") or a drive letter (""C"").");
				}
			}

			this.fileSystem = fileSystem;

			Name = name;
			IsReady = true;
		}

		public new long AvailableFreeSpace { get; set; }
		public new string DriveFormat { get; set; }
		public new DriveType DriveType { get; set; }
		public new bool IsReady { get; protected set; }
		public sealed override string Name { get; protected set; }

		public override DirectoryInfoBase RootDirectory => fileSystem.FromDirectoryName(Name);

		public new long TotalFreeSpace { get; protected set; }
		public new long TotalSize { get; protected set; }
		public override string VolumeLabel { get; set; }
	}
}
