using DragonSpark.Sources;
using DragonSpark.Sources.Scopes;
using JetBrains.Annotations;
using System;
using System.IO;

namespace DragonSpark.Testing.Framework.FileSystem
{
	public sealed class DirectorySource : SingletonScope<IDirectorySource>, IDirectorySource
	{
		public static DirectorySource Default { get; } = new DirectorySource();
		DirectorySource() : base( () => new Implementation() ) {}

		public new string Get() => base.Get().Get();

		public void Assign( string item ) => base.Get().Assign( item );

		public string PathRoot => base.Get().PathRoot;

		public sealed class Implementation : SuppliedSource<string>, IDirectorySource
		{
			public Implementation() : this( Defaults.PathRoot, Guid.NewGuid().ToString() ) {}

			[UsedImplicitly]
			public Implementation( string pathRoot, string name ) : base( Path.Combine( pathRoot, name ) )
			{
				PathRoot = pathRoot;
			}

			public string PathRoot { get; }
		}
	}
}