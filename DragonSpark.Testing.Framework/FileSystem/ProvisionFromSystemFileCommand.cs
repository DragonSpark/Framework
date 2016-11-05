using DragonSpark.Commands;
using DragonSpark.Windows.FileSystem;
using JetBrains.Annotations;
using System;
using System.Collections.Immutable;
using FileInfo = System.IO.FileInfo;

namespace DragonSpark.Testing.Framework.FileSystem
{
	public class ProvisionFromSystemFileCommand : CommandBase<FileInfo>
	{
		public static ProvisionFromSystemFileCommand Default { get; } = new ProvisionFromSystemFileCommand();
		ProvisionFromSystemFileCommand() : this( RegisterFileSystemEntryCommand.Default, PathTranslator.Default.Get, ByteReader.Default.Get ) {}

		readonly ICommand<FileSystemRegistration> register;
		readonly Func<FileInfo, string> pathSource;
		readonly Func<string, ImmutableArray<byte>> reader;
		
		[UsedImplicitly]
		public ProvisionFromSystemFileCommand( ICommand<FileSystemRegistration> register, Func<FileInfo, string> pathSource, Func<string, ImmutableArray<byte>> reader )
		{
			this.register = register;
			this.pathSource = pathSource;
			this.reader = reader;
		}

		public override void Execute( FileInfo parameter )
		{
			var path = pathSource( parameter );
			register.Execute( new FileSystemRegistration( path, reader( parameter.FullName ) ) );
		}
	}
}