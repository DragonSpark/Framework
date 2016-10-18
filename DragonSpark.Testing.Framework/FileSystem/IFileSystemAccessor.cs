using System.Collections.Immutable;
using System.IO.Abstractions;

namespace DragonSpark.Testing.Framework.FileSystem
{
	/// <summary>
	/// Attribution: https://github.com/tathamoddie/System.IO.Abstractions
	/// </summary>
	public interface IFileSystemAccessor
	{
		/// <summary>
		/// Gets a file.
		/// </summary>
		/// <param name="path">The path of the file to get.</param>
		/// <returns>The file. <see langword="null"/> if the file does not exist.</returns>
		IFileSystemElement GetElement(string path);

		void AddFile(string path, IFileElement file);
		void AddDirectory(string path);

		/// <summary>
		/// Removes the file.
		/// </summary>
		/// <param name="path">The file to remove.</param>
		/// <remarks>
		/// The file must not exist.
		/// </remarks>
		void RemoveFile(string path);

		/// <summary>
		/// Determines whether the file exists.
		/// </summary>
		/// <param name="path">The file to check. </param>
		/// <returns><see langword="true"/> if the file exists; otherwise, <see langword="false"/>.</returns>
		bool FileExists(string path);

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

		DirectoryBase Directory { get; }
		IFileInfoFactory FileInfo {get; }
		PathBase Path { get; }
		IDirectoryInfoFactory DirectoryInfo { get; }
		IDriveInfoFactory DriveInfo { get; }

		PathVerifier PathVerifier { get; }
	}
}