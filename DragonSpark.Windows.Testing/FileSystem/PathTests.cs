using DragonSpark.Testing.Framework.FileSystem;
using DragonSpark.Windows.FileSystem;
using System;
using Xunit;

namespace DragonSpark.Windows.Testing.FileSystem
{
	public class PathTests
	{
		[Fact]
		public void Verify()
		{
			Path.Implementation.DefaultImplementation.Assign( () => new Implementation() );
			var path = Path.Current.Get();
			var random = path.GetRandomFileName();
			Assert.Equal( Implementation.Name, random );
		}

		sealed class Implementation : MockPath
		{
			public static string Name { get; } = Guid.NewGuid().ToString();

			public override string GetRandomFileName() => Name;
		}
	}
}