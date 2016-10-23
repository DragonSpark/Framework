using DragonSpark.Sources;
using JetBrains.Annotations;
using System;
using System.IO;

namespace DragonSpark.Testing.Framework.FileSystem
{
	public interface IDirectorySource : IAssignableSource<string>
	{
		string PathRoot { get; }
	}

	public sealed class DirectorySource : SuppliedSource<string>, IDirectorySource
	{
		public static IScope<IDirectorySource> Current { get; } = new Scope<IDirectorySource>( Factory.GlobalCache( () => new DirectorySource() ) );
		DirectorySource() : this( Defaults.PathRoot, Guid.NewGuid().ToString() ) {}

		[UsedImplicitly]
		public DirectorySource( string pathRoot, string name ) : base( Path.Combine( pathRoot, name ) )
		{
			PathRoot = pathRoot;
		}

		public string PathRoot { get; }
	}
}