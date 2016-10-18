using System;
using System.IO;

namespace DragonSpark.Testing.Framework.FileSystem
{
	/// <summary>
	/// Attribution: https://github.com/tathamoddie/System.IO.Abstractions
	/// </summary>
	[Serializable]
	public class DirectoryElement : FileSystemElementBase, IDirectoryElement
	{
		public DirectoryElement()
		{
			Attributes = FileAttributes.Directory;
		}
	}
}