using DragonSpark.Commands;
using JetBrains.Annotations;
using System;
using System.Reflection;
using FileInfo = System.IO.FileInfo;

namespace DragonSpark.Testing.Framework.FileSystem
{
	public class InitializePartsAssemblyCommand : CommandBase<Type>
	{
		public static InitializePartsAssemblyCommand Default { get; } = new InitializePartsAssemblyCommand();
		InitializePartsAssemblyCommand() : this( AssemblySystemFileSource.Default.Get, ProvisionFromSystemFileCommand.Default.Execute ) {}

		readonly Func<Assembly, FileInfo> source;
		readonly Action<FileInfo> provision;

		[UsedImplicitly]
		public InitializePartsAssemblyCommand( Func<Assembly, FileInfo> source, Action<FileInfo> provision )
		{
			this.source = source;
			this.provision = provision;
		}

		public override void Execute( Type parameter )
		{
			var file = source( parameter.Assembly );
			if ( file != null )
			{
				provision( file );
			}
		}
	}
}