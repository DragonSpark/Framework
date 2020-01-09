﻿using DragonSpark.Compose;
using FluentAssertions;
using System;
using Xunit;

namespace DragonSpark.Testing.Application.Compose.Validation
{
	public sealed class EnsureSelectionContextTests
	{
		[Fact]
		void Verify()
		{
			var selector = Start.A.Selection.Of.Type<int>()
			                    .By.Self.Then()
			                    .Ensure.Output.Is(x => x > 3)
			                    .Otherwise.Throw<ArgumentOutOfRangeException>()
			                    .WithMessage("Not greater than Three.")
			                    .Get();
			selector.Invoking(x => x.Get(1))
			        .Should()
			        .Throw<ArgumentOutOfRangeException>()
			        .WithMessage("Specified argument was out of the range of valid values. (Parameter 'Not greater than Three.')");

			selector.Get(4).Should().Be(4);
		}

		[Fact]
		void VerifyUseDefault()
		{
			var selector = Start.A.Selection.Of.Type<int>()
			                    .By.Self.Then()
			                    .Ensure.Output.Is(x => x > 3)
			                    .Otherwise.UseDefault()
			                    .Get();
			selector.Get(3).Should().Be(default);
			selector.Get(4).Should().Be(4);
		}

		[Fact]
		void VerifyUse()
		{
			var selector = Start.A.Selection.Of.Type<int>()
			                    .By.Self.Then()
			                    .Ensure.Output.Is(x => x > 3)
			                    .Otherwise.Use(x => x + 10)
			                    .Get();
			selector.Get(3).Should().Be(13);
			selector.Get(4).Should().Be(4);
		}
	}
}