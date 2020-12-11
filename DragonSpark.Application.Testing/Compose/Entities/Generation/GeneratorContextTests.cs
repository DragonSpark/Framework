﻿using DragonSpark.Application.Compose.Entities.Generation;
using DragonSpark.Compose;
using FluentAssertions;
using JetBrains.Annotations;
using System;
using Xunit;

namespace DragonSpark.Application.Testing.Compose.Entities.Generation
{
	public sealed class GeneratorContextTests
	{
		[Fact]
		public void Verify()
		{
			Start.A.Generator<Basic.Parent>().Include(x => x.Child).Get().Child.Should().NotBeNull();
		}

		[Fact]
		public void VerifyNamed()
		{
			var parent = Start.A.Generator<Named.Parent>().Include(x => x.Child).Get();
			parent.Child.Parent.Should().NotBeNull().And.Subject.Should().BeSameAs(parent);
		}

		[Fact]
		public void VerifyMultiple()
		{
			Start.A.Generator<Multiple.Parent>()
			     .Include(x => x.Child)
			     .Invoking(x => x.Get())
			     .Should()
			     .Throw<InvalidOperationException>();
		}

		static class Basic
		{
			public sealed class Parent
			{
				public Child Child { get; set; } = default!;
			}

			public sealed class Child {}
		}

		static class Multiple
		{
			public sealed class Parent
			{
				public Child Child { get; set; } = default!;

			}

			[UsedImplicitly]
			public sealed class Child
			{
				[UsedImplicitly]
				public Parent Parent1 { get; set; } = default!;
				[UsedImplicitly]
				public Parent Parent2 { get; set; } = default!;
			}
		}

		static class Named
		{
			public sealed class Parent
			{
				public Child Child { get; set; } = default!;

			}

			public sealed class Child
			{
				public Parent Parent { get; set; } = default!;

				[UsedImplicitly]
				public Parent Other { get; set; } = default!;
			}
		}
	}
}