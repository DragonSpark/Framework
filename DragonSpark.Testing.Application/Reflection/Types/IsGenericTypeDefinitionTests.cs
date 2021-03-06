﻿using FluentAssertions;
using DragonSpark.Reflection.Types;
using Xunit;

namespace DragonSpark.Testing.Application.Reflection.Types
{
	public sealed class IsGenericTypeDefinitionTests
	{
		// ReSharper disable once UnusedTypeParameter
		sealed class Subject<T> {}

		[Fact]
		void Verify()
		{
			var sut = IsGenericTypeDefinition.Default;
			sut.Get(typeof(Subject<object>)).Should().BeFalse();
			sut.Get(typeof(Subject<>)).Should().BeTrue();
		}
	}
}