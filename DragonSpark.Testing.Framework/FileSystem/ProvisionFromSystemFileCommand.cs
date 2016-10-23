using DragonSpark.Commands;
using DragonSpark.Sources;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Parameterized.Caching;
using DragonSpark.TypeSystem;
using DragonSpark.Windows.FileSystem;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using FileInfo = System.IO.FileInfo;

namespace DragonSpark.Testing.Framework.FileSystem
{
	public class ProvisionFromSystemFileCommand : CommandBase<FileInfo>
	{
		public static IScope<ProvisionFromSystemFileCommand> Current { get; } = new Scope<ProvisionFromSystemFileCommand>( Factory.GlobalCache( () => new ProvisionFromSystemFileCommand() ) );
		ProvisionFromSystemFileCommand() : this( ApplyFileSystemEntryCommand.Current.Get(), MappedPaths.Current.Get(), PathTranslator.Current.GetCurrentDelegate(), ByteReader.Default.Get ) {}

		readonly ICommand<FileSystemEntry> apply;
		readonly Func<FileInfo, string> pathSource;
		readonly Func<string, ImmutableArray<byte>> reader;
		readonly IAssignableReferenceSource<string, string> mappings;

		[UsedImplicitly]
		public ProvisionFromSystemFileCommand( ICommand<FileSystemEntry> apply, IAssignableReferenceSource<string, string> mappings, Func<FileInfo, string> pathSource, Func<string, ImmutableArray<byte>> reader )
		{
			this.apply = apply;
			this.pathSource = pathSource;
			this.reader = reader;
			this.mappings = mappings;
		}

		public override void Execute( FileInfo parameter )
		{
			var path = pathSource( parameter );
			mappings.Set( parameter.FullName, path );
			apply.Execute( new FileSystemEntry( path, reader( parameter.FullName ) ) );
		}
	}

	public sealed class ApplyFileSystemEntryCommand : CommandBase<FileSystemEntry>
	{
		public static IScope<ApplyFileSystemEntryCommand> Current { get; } = new Scope<ApplyFileSystemEntryCommand>( Factory.GlobalCache( () => new ApplyFileSystemEntryCommand() ) );
		ApplyFileSystemEntryCommand() : this( FileSystemRepository.Current.Get() ) {}

		readonly IFileSystemRepository repository;

		[UsedImplicitly]
		public ApplyFileSystemEntryCommand( IFileSystemRepository repository )
		{
			this.repository = repository;
		}

		public override void Execute( FileSystemEntry parameter ) => repository.Set( parameter.Path, parameter.Element );
	}

	public struct FileSystemEntry
	{
		public static FileSystemEntry File( string path ) => new FileSystemEntry( path, new FileElement( Items<byte>.Default ) );
		public static FileSystemEntry Directory( string path ) => new FileSystemEntry( path, new DirectoryElement() );

		public FileSystemEntry( string path, ImmutableArray<byte> bytes ) : this( path, new FileElement( bytes ) ) {}
		public FileSystemEntry( string path, IEnumerable<byte> bytes ) : this( path, new FileElement( bytes ) ) {}

		public FileSystemEntry( string path, IFileSystemElement element )
		{
			Path = path;
			Element = element;
		}

		public string Path { get; }
		public IFileSystemElement Element { get; }
	}

	public sealed class MappedPathAlteration : AppliedAlteration<string>
	{
		public static IScope<MappedPathAlteration> Current { get; } = new Scope<MappedPathAlteration>( Factory.GlobalCache( () => new MappedPathAlteration() ) );
		MappedPathAlteration() : this( MappedPaths.Current.Get() ) {}

		[UsedImplicitly]
		public MappedPathAlteration( IParameterizedSource<string, string> mappings ) : base( new AlterationAdapter<string>( mappings.ToSourceDelegate() ).Get ) {}
	}

	sealed class MappedPaths : EqualityReferenceCache<string, string>
	{
		public static ISource<MappedPaths> Current { get; } = new Scope<MappedPaths>( Factory.GlobalCache( () => new MappedPaths() ) );
	}
}