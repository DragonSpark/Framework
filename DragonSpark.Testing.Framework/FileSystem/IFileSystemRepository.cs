using System.Collections.Immutable;
using System.IO.Abstractions;
using DragonSpark.Sources.Parameterized.Caching;

namespace DragonSpark.Testing.Framework.FileSystem
{
	public interface IFileSystemRepository : ICache<string, IFileSystemElement>, IFileInfoFactory, IDirectoryInfoFactory, IDriveInfoFactory
	{
		string GetPath( IFileSystemElement element );

		/// <summary>
		/// Gets all unique paths of all files and directories.
		/// </summary>
		ImmutableArray<string> AllPaths { get; }

		/// <summary>
		/// Gets the paths of all files.
		/// </summary>
		ImmutableArray<string> AllFiles { get; }

		/// <summary>
		/// Gets the paths of all directories.
		/// </summary>
		ImmutableArray<string> AllDirectories { get; }
	}
}