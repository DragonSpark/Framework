﻿using FluentAssertions;
using JetBrains.Annotations;
using DragonSpark.Runtime.Activation;
using Xunit;

namespace DragonSpark.Testing.Application.Runtime.Activation
{
	public class HasSingletonPropertyTests
	{
		class Contains
		{
			[UsedImplicitly]
			public static Contains Default { get; } = new Contains();

			Contains() {}
		}

		[Fact]
		public void Is()
		{
			HasSingletonProperty.Default.Get(typeof(Contains))
			                    .Should()
			                    .BeTrue();
		}

		[Fact]
		public void IsNot()
		{
			HasSingletonProperty.Default.Get(GetType())
			                    .Should()
			                    .BeFalse();
		}
	}
}