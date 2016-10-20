using DragonSpark.Extensions;
using DragonSpark.Properties;
using DragonSpark.Runtime;
using DragonSpark.Sources;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Parameterized.Caching;
using DragonSpark.Windows.FileSystem;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using Path = DragonSpark.Windows.FileSystem.Path;

namespace DragonSpark.Testing.Framework.FileSystem
{
	public interface IFileSystemRepository : IParameterizedSource<string, IFileSystemElement>, IComposable<IFileElement>, IComposable<IDirectoryElement>, IFileInfoFactory, IDirectoryInfoFactory, IDriveInfoFactory
	{
		/// <summary>
		/// Removes the file.
		/// </summary>
		/// <param name="pathName">The file to remove.</param>
		/// <remarks>
		/// The file must not exist.
		/// </remarks>
		void RemoveFile(string pathName);

		/// <summary>
		/// Determines whether the file exists.
		/// </summary>
		/// <param name="pathName">The file to check. </param>
		/// <returns><see langword="true"/> if the file exists; otherwise, <see langword="false"/>.</returns>
		bool Contains(string pathName);

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

	/// <summary>
	/// Attribution: https://github.com/tathamoddie/System.IO.Abstractions
	/// </summary>
	public class FileSystemRepository : IFileSystemRepository
	{
		readonly IDictionary<string, IFileSystemElement> elements = new Dictionary<string, IFileSystemElement>( StringComparer.OrdinalIgnoreCase );
		readonly ICache<string, DirectoryInfoBase> directories;
		readonly ICache<string, FileInfoBase> files;
		readonly IPath path;
		

		public static IScope<IFileSystemRepository> Current { get; } = new Scope<IFileSystemRepository>( Factory.GlobalCache( () => new FileSystemRepository() ) );
		FileSystemRepository() : this( Path.Current.Get() ) {}

		[UsedImplicitly]
		public FileSystemRepository( IPath path )
		{
			this.path = path;

			directories = new DirectorySource( this ).ToEqualityCache();
			files = new FileSource( this ).ToEqualityCache();
		}

		public ImmutableArray<string> AllPaths => elements.Keys.ToImmutableArray();

		public ImmutableArray<string> AllFiles => elements.Where( f => f.Value is IFileElement ).Select( f => f.Key ).ToImmutableArray();

		public ImmutableArray<string> AllDirectories => elements.Where( f => f.Value is IDirectoryElement ).Select( f => f.Key ).ToImmutableArray();

		public IFileSystemElement Get( string parameter )
		{
			IFileSystemElement found;
			var result = elements.TryGetValue( path.Normalize( parameter ), out found ) ? found : null;
			return result;
		}

		public void Add( IFileElement file )
		{
			var key = path.Normalize( file.Path );
			if ( Contains( key ) && ( elements[key].Attributes & FileAttributes.ReadOnly ) == FileAttributes.ReadOnly | ( elements[key].Attributes & FileAttributes.Hidden ) == FileAttributes.Hidden )
			{
				throw new UnauthorizedAccessException( string.Format( CultureInfo.InvariantCulture, Properties.Resources.ACCESS_TO_THE_PATH_IS_DENIED, file.Path ) );
			}
			var name = path.Normalize( path.GetDirectoryName( key ) );
			if ( !elements.ContainsKey( name ) )
			{
				Add( new DirectoryElement( name ) );
			}
			elements[key] = file;
		}

		public void Add( IDirectoryElement element )
		{
			var name = path.Normalize( element.Path );
			if ( elements.ContainsKey( name ) && ( elements[name].Attributes & FileAttributes.ReadOnly ) == FileAttributes.ReadOnly )
			{
				throw new UnauthorizedAccessException( string.Format( CultureInfo.InvariantCulture, Properties.Resources.ACCESS_TO_THE_PATH_IS_DENIED, name ) );
			}
			var local4 = 0;
			var separator = path.DirectorySeparatorChar.ToString();
			var prefix = Defaults.IsUnix ? Windows.FileSystem.Defaults.UncUnix : Windows.FileSystem.Defaults.Unc;
			if ( name.StartsWith( prefix, StringComparison.OrdinalIgnoreCase ) )
			{
				local4 = name.IndexOf( separator, 2, StringComparison.OrdinalIgnoreCase );
				if ( local4 < 0 )
				{
					throw new ArgumentException( Resources.SERVER_PATH, nameof( name ) );
				}
			}
			while ( ( local4 = name.IndexOf( separator, local4 + 1, StringComparison.OrdinalIgnoreCase ) ) > -1 )
			{
				var local10 = path.Normalize( name.Substring( 0, local4 + 1 ) );
				if ( !Contains( local10 ) )
				{
					elements[local10] = new DirectoryElement( local10 );
				}
			}
			// var key = name.EndsWith( separator, StringComparison.OrdinalIgnoreCase ) ? name : name + str;
			elements[name] = element.Path.Equals( name, StringComparison.OrdinalIgnoreCase ) ? element : new DirectoryElement( name );
		}

		public void RemoveFile( string pathName ) => elements.Remove( path.Normalize( pathName ) );

		public bool Contains( string pathName ) => elements.ContainsKey( path.Normalize( pathName ) );

		public DirectoryInfoBase FromDirectoryName( string directoryName ) => directories.Get( path.Normalize( directoryName ) );

		public FileInfoBase FromFileName( string fileName ) => files.Get( path.Normalize( fileName ) );

		public DriveInfoBase[] GetDrives()
		{
			var letters = new HashSet<string>(DriveEqualityComparer.Default);
			letters.AddRange(AllPaths.Select( path.GetPathRoot ));

			var result = new List<DriveInfoBase>();
			foreach (var driveLetter in letters)
			{
				try
				{
					result.Add( new MockDriveInfo( this, driveLetter ) );
				}
				catch (ArgumentException) {} // invalid drives should be ignored
			}

			return result.ToArray();
		}
		
		/*string ExtractFileName(string fullFileName) => fullFileName.Split(path.DirectorySeparatorChar).Last();

		string ExtractFilePath(string fullFileName)
		{
			var extractFilePath = fullFileName.Split(path.DirectorySeparatorChar);
			var result = string.Join( path.DirectorySeparatorChar.ToString(), extractFilePath.Take( extractFilePath.Length - 1 ) );
			return result;
		}*/



		sealed class DirectorySource : ParameterizedSourceBase<string, DirectoryInfoBase>
		{
			readonly IFileSystemRepository repository;

			public DirectorySource( IFileSystemRepository repository )
			{
				this.repository = repository;
			}

			public override DirectoryInfoBase Get( string parameter ) => new MockDirectoryInfo( repository, parameter );
		}

		sealed class FileSource : ParameterizedSourceBase<string, FileInfoBase>
		{
			readonly IFileSystemRepository repository;

			public FileSource( IFileSystemRepository repository )
			{
				this.repository = repository;
			}

			public override FileInfoBase Get( string parameter ) => new MockFileInfo( repository, parameter );
		}

		sealed class DriveEqualityComparer : IEqualityComparer<string>
		{
			public static DriveEqualityComparer Default { get; } = new DriveEqualityComparer();
			DriveEqualityComparer() {}

			public bool Equals(string x, string y) => ReferenceEquals( x, y ) || !ReferenceEquals( x, null ) && ( !ReferenceEquals( y, null ) && ( x[1] == ':' && y[1] == ':' && char.ToUpperInvariant( x[0] ) == char.ToUpperInvariant( y[0] ) ) );

			public int GetHashCode(string obj) => obj.ToUpperInvariant().GetHashCode();
		}
	}
}