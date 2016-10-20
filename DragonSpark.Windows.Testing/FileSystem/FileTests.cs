using DragonSpark.Testing.Framework.Application;
using DragonSpark.Testing.Framework.FileSystem;
using DragonSpark.Windows.FileSystem;
using System;
using Xunit;

namespace DragonSpark.Windows.Testing.FileSystem
{
	public class FileTests
	{
		[Theory, AutoData]
		public void Verify( string path )
		{
			File.Implementation.DefaultImplementation.Assign( () => new Implementation() );
			var sut = File.Current.Get();
			var random = sut.ReadAllText( path );
			Assert.Equal( string.Concat( path, Implementation.Name ), random );
		}

		sealed class Implementation : MockFile
		{
			public static string Name { get; } = Guid.NewGuid().ToString();

			public override string ReadAllText( string pathName ) => string.Concat( pathName, Name );
		}
	}
}