using DragonSpark.Commands;
using DragonSpark.Sources;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Parameterized.Caching;
using DragonSpark.Windows.FileSystem;
using JetBrains.Annotations;
using System;
using System.Collections.Immutable;
using FileInfo = System.IO.FileInfo;

namespace DragonSpark.Testing.Framework.FileSystem
{
	public class ProvisionFromSystemFileCommand : CommandBase<FileInfo>
	{
		public static IScope<ProvisionFromSystemFileCommand> Current { get; } = Scopes.Create( () => new ProvisionFromSystemFileCommand() );
		ProvisionFromSystemFileCommand() : this( RegisterFileSystemEntryCommand.Current.Get(), MappedPaths.Current.Get(), PathTranslator.Current.GetValue, ByteReader.Default.Get ) {}

		readonly ICommand<FileSystemRegistration> apply;
		readonly Func<FileInfo, string> pathSource;
		readonly Func<string, ImmutableArray<byte>> reader;
		readonly IAssignableReferenceSource<string, string> mappings;

		[UsedImplicitly]
		public ProvisionFromSystemFileCommand( ICommand<FileSystemRegistration> apply, IAssignableReferenceSource<string, string> mappings, Func<FileInfo, string> pathSource, Func<string, ImmutableArray<byte>> reader )
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
			apply.Execute( new FileSystemRegistration( path, reader( parameter.FullName ) ) );
		}
	}

	public sealed class MappedPathAlteration : AppliedAlteration<string>
	{
		public static IScope<MappedPathAlteration> Current { get; } = Scopes.Create( () => new MappedPathAlteration() );
		MappedPathAlteration() : this( MappedPaths.Current.Get() ) {}

		[UsedImplicitly]
		public MappedPathAlteration( IParameterizedSource<string, string> mappings ) : base( new AlterationAdapter<string>( mappings.ToDelegate() ).Get ) {}
	}

	sealed class MappedPaths : EqualityReferenceCache<string, string>
	{
		public static ISource<MappedPaths> Current { get; } = Scopes.Create( () => new MappedPaths() );
	}
}