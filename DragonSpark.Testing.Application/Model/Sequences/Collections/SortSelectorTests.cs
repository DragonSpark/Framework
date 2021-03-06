﻿using FluentAssertions;
using DragonSpark.Model.Sequences.Collections;
using Xunit;

namespace DragonSpark.Testing.Application.Model.Sequences.Collections
{
	public sealed class SortSelectorTests
	{
		sealed class Subject {}

		sealed class Aware : ISortAware
		{
			public int Get() => 6776;
		}

		[Sort(123)]
		sealed class Declared {}

		[Fact]
		void Verify()
		{
			SortSelector<Subject>.Default.Get(new Subject()).Should().Be(-1);
		}

		[Fact]
		void VerifyAware()
		{
			SortSelector<Aware>.Default.Get(new Aware()).Should().Be(6776);
		}

		[Fact]
		void VerifyDeclared()
		{
			SortSelector<Declared>.Default.Get(new Declared()).Should().Be(123);
		}
	}
}