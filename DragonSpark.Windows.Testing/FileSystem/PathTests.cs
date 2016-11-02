﻿using DragonSpark.Testing.Framework.FileSystem;
using DragonSpark.Windows.FileSystem;
using Moq;
using Xunit;
using Factory = DragonSpark.Sources.Scopes.Factory;

namespace DragonSpark.Windows.Testing.FileSystem
{
	public class PathTests
	{
		[Fact]
		public void Verify()
		{
			Path.Default.Configuration.Assign( Factory.Cache( () => new Mock<MockPath> { CallBase = true }.Object ) );

			var implementation = Path.Default.Configuration.Get();
			Assert.Same( Path.Default.Configuration.Get(), implementation );
			var mock = Mock.Get( (MockPath)implementation );
			var instance = Path.Default.Get();
			Assert.Same( Path.Default.Get(), instance  );
			mock.Verify( i => i.GetRandomFileName(), Times.Never() );
			Assert.NotEmpty( instance.GetRandomFileName() );
			mock.Verify( i => i.GetRandomFileName(), Times.Once() );
		}
	}
}