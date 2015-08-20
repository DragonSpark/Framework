﻿using DragonSpark.Testing.TestObjects;
using Moq;
using Ploeh.AutoFixture.Xunit2;
using Xunit;

namespace DragonSpark.Testing.Framework.Testing
{
	public class AutoMoqDataAttributeTests
	{
		[Theory, AutoMockData]
		public void Mocked( [Frozen]Mock<IInterface> sut, IInterface item )
		{
			Assert.Equal( sut.Object, item );
		}
	}
}
