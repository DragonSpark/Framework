﻿using DragonSpark.Model.Sequences.Collections;
using FluentAssertions;
using Xunit;

namespace DragonSpark.Testing.Model.Sequences.Collections
{
	public sealed class KeyedByTypeCollectionTests
	{
		public interface ISubject {}

		sealed class Subject : ISubject {}

		sealed class Other : ISubject {}

		[Fact]
		public void Verify()
		{
			var first  = new Subject();
			var second = new Subject();
			var other  = new Other();

			var sut = new KeyedByTypeCollection<ISubject>();
			sut.Add(first);
			sut.Add(second);
			sut.Add(other);

			sut[typeof(Other)].Should().BeSameAs(other);
			sut[typeof(Subject)].Should().BeSameAs(second);
		}
	}
}