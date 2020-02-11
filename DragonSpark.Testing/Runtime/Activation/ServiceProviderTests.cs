using AutoFixture.Xunit2;
using DragonSpark.Compose;
using DragonSpark.Runtime.Activation;
using FluentAssertions;
using System;
using Xunit;

namespace DragonSpark.Testing.Runtime.Activation
{
	public class ServiceProviderTests
	{
		[Theory, AutoData]
		public void Verify(int number)
		{
			var sut = new ServiceProvider(number);
			sut.Get(typeof(DateTime))
			   .Should()
			   .BeFalse();
			sut.Get(typeof(int))
			   .Should()
			   .BeTrue();

			sut.Get<int>()
			   .Should()
			   .Be(number);

			sut.Get<DateTime?>()
			   .Should()
			   .BeNull();
		}
	}
}