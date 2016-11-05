using DragonSpark.Extensions;
using DragonSpark.Windows.FileSystem;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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
	public class MockPath : PathWrapper, IPath
	{
		readonly IDirectorySource directory;
		readonly char[] separator;
		readonly string[] separators;
		readonly string directorySeparator;


		public MockPath() : this( DirectorySource.Default ) {}

		[UsedImplicitly]
		public MockPath(IDirectorySource directory)
		{
			this.directory = directory;

			var c = DirectorySeparatorChar;
			directorySeparator = c.ToString( CultureInfo.InvariantCulture );
			separator = new[] { c };
			separators = new[] { directorySeparator, AltDirectorySeparatorChar.ToString( CultureInfo.InvariantCulture ) };
		}

		public sealed override char DirectorySeparatorChar => Defaults.IsUnix ? AltDirectorySeparatorChar : base.DirectorySeparatorChar;

		public sealed override char AltDirectorySeparatorChar => Defaults.IsUnix ? DirectorySeparatorChar : base.AltDirectorySeparatorChar;

		public override string GetPathRoot( string path ) => base.GetPathRoot( path ).NullIfEmpty() ?? ( path.StartsWith( directory.PathRoot, StringComparison.OrdinalIgnoreCase ) ? directory.PathRoot : string.Empty );

		public override string GetFullPath(string path)
		{
			var replaced = path.Replace(AltDirectorySeparatorChar, DirectorySeparatorChar);
			var isUnc = replaced.StartsWith( Defaults.IsUnix ? Windows.FileSystem.Defaults.UncUnix : Windows.FileSystem.Defaults.Unc, StringComparison.OrdinalIgnoreCase );
			var root = GetPathRoot( path );

			var current = directory.Get();
			var segments = root == string.Empty // relative path on the current drive or volume
				? 
				GetSegments( Combine( current, replaced ) ) // absolute path on the current drive or volume
					: 
					separators.Any( s => s.Equals( root, StringComparison.OrdinalIgnoreCase ) ) ? GetSegments( directory.PathRoot, replaced ) : GetSegments( replaced );

			if ( isUnc && segments.Length < 2 )
			{
				throw new ArgumentException( @"The UNC path should be of the form \\server\share.", nameof( path ) );
			}
			
			// unc paths need at least two segments, the others need one segment
			var isUnixRooted = Defaults.IsUnix && current.StartsWith( directorySeparator, StringComparison.OrdinalIgnoreCase );

			var minPathSegments = isUnc ? 2 : isUnixRooted ? 0 : 1;

			var stack = new Stack<string>();
			foreach ( var segment in segments )
			{
				if ( Windows.FileSystem.Defaults.ParentPath.Equals( segment, StringComparison.OrdinalIgnoreCase ) )
				{
					if (stack.Count > minPathSegments) // only pop, if afterwards are at least the minimal amount of path segments
					{
						stack.Pop();
					}
				}
				else if ( Windows.FileSystem.Defaults.CurrentPath.Equals( segment, StringComparison.OrdinalIgnoreCase ) ) {} // ignore .
				else
				{
					stack.Push( segment );
				}
			}

			var joined = this.EnsureTrailingSlash( Combine( stack.Reverse().ToArray() ), replaced );

			var prefix = isUnixRooted 
				? 
				isUnc ? Windows.FileSystem.Defaults.UncUnix : directorySeparator 
				: 
				( isUnc ? Windows.FileSystem.Defaults.Unc : string.Empty );

			var result = string.Concat( prefix, joined );
			return result;
		}

		ImmutableArray<string> GetSegments( params string[] paths )
		{
			var segments = paths.SelectMany( path => path.Split( separator, StringSplitOptions.RemoveEmptyEntries ) ).ToArray();
			if ( segments.Length > 1 )
			{
				var first = segments[0];
				segments[0] = directory.PathRoot.StartsWith( first ) ? this.EnsureTrailingSlash( first ) : first;
			}
			
			var result = segments.ToImmutableArray();
			return result;
		}
	}
}
