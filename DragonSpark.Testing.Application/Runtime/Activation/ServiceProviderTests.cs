using System;
using AutoFixture.Xunit2;
using FluentAssertions;
using DragonSpark.Runtime.Activation;
using Xunit;

namespace DragonSpark.Testing.Application.Runtime.Activation
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