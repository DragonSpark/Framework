using System;
using System.IO.Abstractions;

namespace DragonSpark.Testing.Framework.FileSystem
{
	/// <summary>
	/// Attribution: https://github.com/tathamoddie/System.IO.Abstractions
	/// </summary>
	[Serializable]
	public class MockDirectoryInfoFactory : IDirectoryInfoFactory
	{
		readonly IFileSystemAccessor fileSystem;

		public MockDirectoryInfoFactory(IFileSystemAccessor fileSystem)
		{
			this.fileSystem = fileSystem;
		}

		public DirectoryInfoBase FromDirectoryName(string directoryName) => new MockDirectoryInfo(fileSystem, directoryName);
	}
}