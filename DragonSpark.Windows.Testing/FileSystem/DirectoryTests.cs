using DragonSpark.Testing.Framework.FileSystem;
using DragonSpark.Windows.FileSystem;
using System;
using Xunit;

namespace DragonSpark.Windows.Testing.FileSystem
{
	public class DirectoryTests
	{
		[Fact]
		public void Verify()
		{
			Directory.Implementation.DefaultImplementation.Assign( () => new Implementation() );
			var sut = Directory.Current.Get();
			var random = sut.GetCurrentDirectory();
			Assert.Equal( Implementation.Name, random );
		}

		sealed class Implementation : MockDirectory
		{
			public static string Name { get; } = Guid.NewGuid().ToString();

			public override string GetCurrentDirectory() => Name;
		}
	}
}