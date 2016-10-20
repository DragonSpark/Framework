using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO.Abstractions;
using System.Linq;

namespace DragonSpark.Testing.Framework.FileSystem
{
	/// <summary>
	/// PathWrapper calls direct to Path but all this does is string manipulation so we can inherit directly from PathWrapper as no IO is done
	/// Attribution: https://github.com/tathamoddie/System.IO.Abstractions
	/// </summary>
	[Serializable]
	public class MockPath : PathWrapper
	{
		readonly IDirectorySource directory;
		readonly char[] separator;
		readonly string separatorText;

		public MockPath() : this( DirectorySource.Current.Get() ) {}

		[UsedImplicitly]
		public MockPath(IDirectorySource directory)
		{
			this.directory = directory;

			var c = DirectorySeparatorChar;
			separatorText = c.ToString(CultureInfo.InvariantCulture);
			separator = new[] { c };
		}

		public sealed override char DirectorySeparatorChar => Defaults.IsUnix ? AltDirectorySeparatorChar : base.DirectorySeparatorChar;

		public override char AltDirectorySeparatorChar => Defaults.IsUnix ? DirectorySeparatorChar : base.AltDirectorySeparatorChar;

		public override string GetFullPath(string path)
		{
			var directorySeparator = DirectorySeparatorChar;
			var altDirectorySeparatorChar = AltDirectorySeparatorChar;
			var replaced = path.Replace(altDirectorySeparatorChar, directorySeparator);
			var isUnc = replaced.StartsWith( Windows.FileSystem.Defaults.Unc, StringComparison.OrdinalIgnoreCase) || replaced.StartsWith( Windows.FileSystem.Defaults.UncUnix, StringComparison.OrdinalIgnoreCase);
			var root = GetPathRoot(path);

			string[] pathSegments;

			if (root.Length == 0)
			{
				// relative path on the current drive or volume
				pathSegments = GetSegments($"{directory.Get()}{separatorText}{replaced}");
			}
			else if (isUnc)
			{
				// unc path
				pathSegments = GetSegments(replaced);
				if (pathSegments.Length < 2)
				{
					throw new ArgumentException(@"The UNC path should be of the form \\server\share.", nameof( path ));
				}
			}
			else if (separatorText.Equals(root, StringComparison.OrdinalIgnoreCase) || altDirectorySeparatorChar.ToString().Equals(root, StringComparison.OrdinalIgnoreCase))
			{
				// absolute path on the current drive or volume
				pathSegments = GetSegments(GetPathRoot(directory.Get()), replaced);
			}
			else
			{
				pathSegments = GetSegments(replaced);
			}

			// unc paths need at least two segments, the others need one segment
			var isUnixRooted = directory.Get().StartsWith(separatorText, StringComparison.OrdinalIgnoreCase);

			var minPathSegments = isUnc ? 2 : isUnixRooted ? 0 : 1;

			var stack = new Stack<string>();
			foreach (var segment in pathSegments)
			{
				if ( Windows.FileSystem.Defaults.ParentPath.Equals(segment, StringComparison.OrdinalIgnoreCase))
				{
					// only pop, if afterwards are at least the minimal amount of path segments
					if (stack.Count > minPathSegments)
					{
						stack.Pop();
					}
				}
				else if ( Windows.FileSystem.Defaults.CurrentPath.Equals(segment, StringComparison.OrdinalIgnoreCase)) {} // ignore .
				else
				{
					stack.Push(segment);
				}
			}

			var joined = string.Join(separatorText, stack.Reverse().ToArray());
			var fullPath = replaced.Length > 1 && replaced.EndsWith( separatorText, StringComparison.OrdinalIgnoreCase ) ? string.Concat( joined, separatorText ) : joined;

			var result = isUnixRooted 
				? 
				string.Concat( isUnc ? Windows.FileSystem.Defaults.UncUnix : separatorText, fullPath ) 
				: 
				( isUnc ? string.Concat( Windows.FileSystem.Defaults.Unc, fullPath ) : fullPath );
			return result;
		}

		string[] GetSegments( params string[] paths ) => paths.SelectMany( path => path.Split( separator, StringSplitOptions.RemoveEmptyEntries ) ).ToArray();
	}
}
