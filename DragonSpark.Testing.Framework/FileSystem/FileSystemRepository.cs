using DragonSpark.Commands;
using DragonSpark.Extensions;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Parameterized.Caching;
using DragonSpark.Sources.Scopes;
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
	/// <summary>
	/// Attribution: https://github.com/tathamoddie/System.IO.Abstractions
	/// </summary>
	public class FileSystemRepository : SingletonScope<IFileSystemRepository>, IFileSystemRepository
	{
		public static FileSystemRepository Default { get; } = new FileSystemRepository();
		FileSystemRepository() : base( () => new Implementation() ) {}

		public string GetPath( IFileSystemElement element ) => Get().GetPath( element );
		public ImmutableArray<string> AllPaths => Get().AllPaths;
		public ImmutableArray<string> AllFiles => Get().AllFiles;
		public ImmutableArray<string> AllDirectories => Get().AllDirectories;
		public DirectoryInfoBase FromDirectoryName( string directoryName ) => Get().FromDirectoryName( directoryName );
		public FileInfoBase FromFileName( string fileName ) => Get().FromFileName( fileName );
		public DriveInfoBase[] GetDrives() => Get().GetDrives();
		public IFileSystemElement Get( string parameter ) => Get().Get( parameter );
		public void Set( string instance, IFileSystemElement value ) => Get().Set( instance, value );
		public bool Contains( string instance ) => Get().Contains( instance );
		public bool Remove( string instance ) => Get().Remove( instance );

		sealed class Implementation : AlteredCache<string, IFileSystemElement>, IFileSystemRepository
		{
			readonly IDictionary<string, IFileSystemElement> elements;
			readonly ICache<string, DirectoryInfoBase> directories;
			readonly ICache<string, FileInfoBase> files;
			readonly IPath path;

			public Implementation() : this( Path.Default ) {}

			[UsedImplicitly]
			public Implementation( IPath path ) : this( path, new Dictionary<string, IFileSystemElement>( StringComparer.OrdinalIgnoreCase ) ) {}

			[UsedImplicitly]
			public Implementation( IPath path, IDictionary<string, IFileSystemElement> elements ) : base( new Cache( path, elements ), new DelegatedAlteration<string>( path.Normalize ).Get )
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
				var letters = new HashSet<string>( DriveEqualityComparer.Instance );
				letters.AddRange( AllPaths.Select( path.GetPathRoot ) );

				var result = new List<DriveInfoBase>();
				foreach ( var driveLetter in letters )
				{
					try
					{
						result.Add( new MockDriveInfo( this, driveLetter ) );
					}
					catch ( ArgumentException ) {} // invalid drives should be ignored
				}

				return result.ToArray();
			}
		}

		sealed class Cache : DictionaryCache<string, IFileSystemElement>
		{
			readonly IDictionary<Type, ICommand<string>> commands;

			public Cache( IPath path, IDictionary<string, IFileSystemElement> dictionary ) : this( PreparedCommandFactory.Instance.Get( new PreparedCommandFactoryParameter( path, dictionary ) ), dictionary ) {}

			Cache( IDictionary<Type, ICommand<string>> commands, IDictionary<string, IFileSystemElement> dictionary ) : base( dictionary )
			{
				this.commands = commands;
			}

			sealed class PreparedCommandFactory : ParameterizedSourceBase<PreparedCommandFactoryParameter, IDictionary<Type, ICommand<string>>>
			{
				readonly static IEqualityComparer<Type> Comparer = new AssignableEqualityComparer( typeof(IDirectoryElement), typeof(IFileElement) );

				public static PreparedCommandFactory Instance { get; } = new PreparedCommandFactory();
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
			public static DriveEqualityComparer Instance { get; } = new DriveEqualityComparer();
			DriveEqualityComparer() {}

			public bool Equals(string x, string y) => ReferenceEquals( x, y ) || !ReferenceEquals( x, null ) && ( !ReferenceEquals( y, null ) && ( x[1] == ':' && y[1] == ':' && char.ToUpperInvariant( x[0] ) == char.ToUpperInvariant( y[0] ) ) );

			public int GetHashCode(string obj) => obj.ToUpperInvariant().GetHashCode();
		}
	}
}