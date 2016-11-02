﻿using DragonSpark.Testing.Framework.FileSystem;
using DragonSpark.Windows.FileSystem;
using Moq;
using Xunit;
using Factory = DragonSpark.Sources.Scopes.Factory;

namespace DragonSpark.Windows.Testing.FileSystem
{
	public class DirectoryTests
	{
		[Fact]
		public void Verify()
		{
			Directory.Default.Assign( Factory.Cache(  () => new Mock<MockDirectory> { CallBase = true }.Object ) );
			var implementation = Directory.Default.Get();
			Assert.Same( Directory.Default.Get(), implementation );
			var mock = Mock.Get( (MockDirectory)implementation );
			var instance = Directory.Default.Get();
			Assert.Same( instance, Directory.Default.Get() );
			mock.Verify( i => i.GetCurrentDirectory(), Times.Never() );

			var item = instance.GetCurrentDirectory();
			Assert.NotEmpty( item );
			mock.Verify( i => i.GetCurrentDirectory(), Times.Once );
		}
	}
}