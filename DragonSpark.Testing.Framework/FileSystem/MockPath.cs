using DragonSpark.Windows.FileSystem;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO.Abstractions;
using System.Linq;
using Path = System.IO.Path;

namespace DragonSpark.Testing.Framework.FileSystem
{
	/// <summary>
	/// PathWrapper calls direct to Path but all this does is string manipulation so we can inherit directly from PathWrapper as no IO is done
	/// Attribution: https://github.com/tathamoddie/System.IO.Abstractions
	/// </summary>
	[Serializable]
	public class MockPath : PathWrapper
	{
		readonly private static char[] InvalidAdditionalPathChars = { '*', '?' };

		public MockPath() : this( FileSystemRepository.Current.Get(), Directory.Current.Get() ) {}

		readonly IFileSystemRepository repository;
		readonly IDirectory directory;

		public MockPath(IFileSystemRepository repository, IDirectory directory)
		{
			this.repository = repository;
			this.directory = directory;
		}

		public override string GetFullPath(string path)
		{
			if (path.Length == 0)
			{
				throw new ArgumentException(Properties.Resources.THE_PATH_IS_NOT_OF_A_LEGAL_FORM, "path");
			}

			path = path.Replace(AltDirectorySeparatorChar, DirectorySeparatorChar);

			bool isUnc =
				path.StartsWith(@"\\", StringComparison.OrdinalIgnoreCase) ||
				path.StartsWith(@"//", StringComparison.OrdinalIgnoreCase);

			string root = GetPathRoot(path);

			bool hasTrailingSlash = path.Length > 1 && path[path.Length - 1] == DirectorySeparatorChar;

			string[] pathSegments;

			if (root.Length == 0)
			{
				// relative path on the current drive or volume
				path = directory.GetCurrentDirectory() + DirectorySeparatorChar + path;
				pathSegments = GetSegments(path);
			}
			else if (isUnc)
			{
				// unc path
				pathSegments = GetSegments(path);
				if (pathSegments.Length < 2)
				{
					throw new ArgumentException(@"The UNC path should be of the form \\server\share.", "path");
				}
			}
			else if (@"\".Equals(root, StringComparison.OrdinalIgnoreCase) || @"/".Equals(root, StringComparison.OrdinalIgnoreCase))
			{
				// absolute path on the current drive or volume
				pathSegments = GetSegments(GetPathRoot(directory.GetCurrentDirectory()), path);
			}
			else
			{
				pathSegments = GetSegments(path);
			}

			// unc paths need at least two segments, the others need one segment
			bool isUnixRooted =
				directory.GetCurrentDirectory()
					.StartsWith(DirectorySeparatorChar.ToString(CultureInfo.InvariantCulture), StringComparison.OrdinalIgnoreCase);

			var minPathSegments = isUnc
				? 2
				: isUnixRooted ? 0 : 1;

			var stack = new Stack<string>();
			foreach (var segment in pathSegments)
			{
				if ("..".Equals(segment, StringComparison.OrdinalIgnoreCase))
				{
					// only pop, if afterwards are at least the minimal amount of path segments
					if (stack.Count > minPathSegments)
					{
						stack.Pop();
					}
				}
				else if (".".Equals(segment, StringComparison.OrdinalIgnoreCase))
				{
					// ignore .
				}
				else
				{
					stack.Push(segment);
				}
			}

			var fullPath = string.Join(DirectorySeparatorChar.ToString(CultureInfo.InvariantCulture), stack.Reverse().ToArray());

			if (hasTrailingSlash)
			{
				fullPath += DirectorySeparatorChar;
			}

			if (isUnixRooted && !isUnc)
			{
				fullPath = DirectorySeparatorChar + fullPath;
			}
			else if (isUnixRooted)
			{
				fullPath = @"//" + fullPath;
			}
			else if (isUnc)
			{
				fullPath = @"\\" + fullPath;
			}

			return fullPath;
		}

		private string[] GetSegments(params string[] paths) => paths.SelectMany(path => path.Split(new[] { DirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries)).ToArray();

		public override string GetTempFileName()
		{
			var fullPath = Combine( GetTempPath(), GetRandomFileName() );
			repository.AddFile( FileElement.Empty( fullPath ) );

			return fullPath;
		}

		internal static bool HasIllegalCharacters(string path, bool checkAdditional)
		{
			if (checkAdditional)
			{
				return path.IndexOfAny(Path.GetInvalidPathChars().Concat(InvalidAdditionalPathChars).ToArray()) >= 0;
			}

			return path.IndexOfAny(Path.GetInvalidPathChars()) >= 0;
		}

		internal static void CheckInvalidPathChars(string path, bool checkAdditional = false)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}

			if (HasIllegalCharacters(path, checkAdditional))
			{
				throw new ArgumentException(Properties.Resources.ILLEGAL_CHARACTERS_IN_PATH_EXCEPTION);
			}
		}
	}
}
