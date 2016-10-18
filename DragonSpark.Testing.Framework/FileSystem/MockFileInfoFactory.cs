using System;
using System.IO.Abstractions;

namespace DragonSpark.Testing.Framework.FileSystem
{
	/// <summary>
	/// Attribution: https://github.com/tathamoddie/System.IO.Abstractions
	/// </summary>
	[Serializable]
    public class MockFileInfoFactory : IFileInfoFactory
    {
        private readonly IFileSystemAccessor fileSystem;

        public MockFileInfoFactory(IFileSystemAccessor fileSystem)
        {
            if (fileSystem == null)
            {
                throw new ArgumentNullException("fileSystem");
            }

            this.fileSystem = fileSystem;
        }

        public FileInfoBase FromFileName(string fileName)
        {
            return new MockFileInfo(fileSystem, fileName);
        }
    }
}