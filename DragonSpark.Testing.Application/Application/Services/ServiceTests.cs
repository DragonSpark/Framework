using DragonSpark.Services.Communication;
using FluentAssertions;
using System;
using Xunit;

namespace DragonSpark.Testing.Application.Application.Services
{
	public sealed class ServiceTests
	{
		[Fact]
		void Verify()
		{
			ClientStore.Default.Get(new Uri("http://microsoft.com"))
			           .Should()
			           .BeSameAs(ClientStore.Default.Get(new Uri("http://microsoft.com")));
		}
	}
}