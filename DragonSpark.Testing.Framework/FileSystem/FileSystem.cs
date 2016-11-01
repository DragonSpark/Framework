using DragonSpark.Commands;
using DragonSpark.Extensions;
using DragonSpark.Sources;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Parameterized.Caching;
using DragonSpark.TypeSystem;
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
	public interface IFileSystemRepository : ICache<string, IFileSystemElement>, /*IComposable<FileEntry>,*/ IFileInfoFactory, IDirectoryInfoFactory, IDriveInfoFactory
	{
		// IFileElement AddFile( string path, IEnumerable<byte> data );

		/*void Remove( IFileSystemElement element );

		/// <summary>
		/// Determines whether the file exists.
		/// </summary>
		/// <param name="pathName">The file to check. </param>
		/// <returns><see langword="true"/> if the file exists; otherwise, <see langword="false"/>.</returns>
		bool Contains(string pathName);*/

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

	/// <summary>
	/// Attribution: https://github.com/tathamoddie/System.IO.Abstractions
	/// </summary>
	public class FileSystemRepository : AlteredCache<string, IFileSystemElement>, IFileSystemRepository
	{
		public static IScope<IFileSystemRepository> Current { get; } = new Scope<IFileSystemRepository>( Factory.GlobalCache( () => new FileSystemRepository() ) );
		FileSystemRepository() : this( Path.Default ) {}

		readonly IDictionary<string, IFileSystemElement> elements;
		readonly ICache<string, DirectoryInfoBase> directories;
		readonly ICache<string, FileInfoBase> files;
		readonly IPath path;

		[UsedImplicitly]
		public FileSystemRepository( IPath path ) : this( path, new Dictionary<string, IFileSystemElement>( StringComparer.OrdinalIgnoreCase ) ) {}

		[UsedImplicitly]
		public FileSystemRepository( IPath path, IDictionary<string, IFileSystemElement> elements ) : base( new Cache( path, elements ), new DelegatedAlteration<string>( path.Normalize ).Get )
		{
			this.path = path;
			this.elements = elements;

			directories = new DirectorySource( this ).ToEqualityCache();
			files = new FileSource( this ).ToEqualityCache();
		}

		public string GetPath( IFileSystemElement element ) => elements.Introduce( element, tuple => tuple.Item1.Value == tuple.Item2, tuple => tuple.Item1.Key ).SingleOrDefault();

		public ImmutableArray<string> AllPaths => elements.Keys.ToImmutableArray();

		public ImmutableArray<string> AllFiles => elements.Where( f => f.Value is IFileElement ).Select( f => f.Key ).ToImmutableArray();

		public ImmutableArray<string> AllDirectories => elements.Where( f => f.Value is IDirectoryElement ).Select( f => f.Key ).ToImmutableArray();

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
		
		sealed class Cache : DictionaryCache<string, IFileSystemElement>
		{
			readonly IDictionary<Type, ICommand<string>> commands;

			public Cache( IPath path, IDictionary<string, IFileSystemElement> dictionary ) : this( PreparedCommandFactory.Default.Get( new PreparedCommandFactoryParameter( path, dictionary ) ), dictionary ) {}

			Cache( IDictionary<Type, ICommand<string>> commands, IDictionary<string, IFileSystemElement> dictionary ) : base( dictionary )
			{
				this.commands = commands;
			}

			sealed class PreparedCommandFactory : ParameterizedSourceBase<PreparedCommandFactoryParameter, IDictionary<Type, ICommand<string>>>
			{
				readonly static IEqualityComparer<Type> Comparer = new AssignableEqualityComparer( typeof(IDirectoryElement), typeof(IFileElement) );

				public static PreparedCommandFactory Default { get; } = new PreparedCommandFactory();
				PreparedCommandFactory() {}

				public override IDictionary<Type, ICommand<string>> Get( PreparedCommandFactoryParameter parameter ) =>
					new Dictionary<Type, ICommand<string>>( Comparer )
					{
						{ typeof(IDirectoryElement), new PrepareDirectoryCommand( parameter.Path, parameter.Dictionary ) },
						{ typeof(IFileElement), new PrepareFileCommand( parameter.Path, parameter.Dictionary ) }
					};

				
			}

			struct PreparedCommandFactoryParameter
			{
				public PreparedCommandFactoryParameter( IPath path, IDictionary<string, IFileSystemElement> dictionary )
				{
					Path = path;
					Dictionary = dictionary;
				}

				public IPath Path { get; }
				public IDictionary<string, IFileSystemElement> Dictionary { get; }
			}

			abstract class PrepareCommandBase : CommandBase<string>
			{
				protected PrepareCommandBase( IPath path, IDictionary<string, IFileSystemElement> dictionary )
				{
					Path = path;
					Dictionary = dictionary;
				}

				protected IPath Path { get; }
				protected IDictionary<string, IFileSystemElement> Dictionary { get; }
			}

			class PrepareDirectoryCommand : PrepareCommandBase
			{
				public PrepareDirectoryCommand( IPath path, IDictionary<string, IFileSystemElement> dictionary ) : base( path, dictionary ) {}

				public override void Execute( string parameter )
				{
					if ( Dictionary.ContainsKey( parameter ) && ( Dictionary[parameter].Attributes & FileAttributes.ReadOnly ) == FileAttributes.ReadOnly )
					{
						throw new UnauthorizedAccessException( string.Format( CultureInfo.InvariantCulture, Properties.Resources.ACCESS_TO_THE_PATH_IS_DENIED, parameter ) );
					}
					var local4 = 0;
					var separator = Path.DirectorySeparatorChar.ToString();
					var prefix = Defaults.IsUnix ? Windows.FileSystem.Defaults.UncUnix : Windows.FileSystem.Defaults.Unc;
					if ( parameter.StartsWith( prefix, StringComparison.OrdinalIgnoreCase ) )
					{
						local4 = parameter.IndexOf( separator, 2, StringComparison.OrdinalIgnoreCase );
						if ( local4 < 0 )
						{
							throw new ArgumentException( DragonSpark.Properties.Resources.SERVER_PATH, nameof( parameter ) );
						}
					}
					while ( ( local4 = parameter.IndexOf( separator, local4 + 1, StringComparison.OrdinalIgnoreCase ) ) > -1 )
					{
						var local10 = Path.Normalize( parameter.Substring( 0, local4 + 1 ) );
						Dictionary.Ensure( local10, s => new DirectoryElement() );
					}
				}
			}

			sealed class PrepareFileCommand : PrepareDirectoryCommand
			{
				public PrepareFileCommand( IPath path, IDictionary<string, IFileSystemElement> dictionary ) : base( path, dictionary ) {}

				public override void Execute( string parameter )
				{
					if ( Dictionary.ContainsKey( parameter ) && ( Dictionary[parameter].Attributes & FileAttributes.ReadOnly ) == FileAttributes.ReadOnly | ( Dictionary[parameter].Attributes & FileAttributes.Hidden ) == FileAttributes.Hidden )
					{
						throw new UnauthorizedAccessException( string.Format( CultureInfo.InvariantCulture, Properties.Resources.ACCESS_TO_THE_PATH_IS_DENIED, parameter ) );
					}
					var name = Path.Normalize( Path.GetDirectoryName( parameter ) );
					if ( !Dictionary.ContainsKey( name ) )
					{
						Dictionary.Add( name, new DirectoryElement() );
						base.Execute( name );
					}
				}
			}

			public override void Set( string instance, IFileSystemElement value )
			{
				commands.TryGet( value.GetType() )?.Execute( instance );
				base.Set( instance, value );
			}
		}


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