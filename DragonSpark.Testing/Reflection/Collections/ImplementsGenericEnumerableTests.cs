using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;
using DragonSpark.Reflection.Collections;
using FluentAssertions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DragonSpark.Testing.Reflection.Collections
{
	public class ImplementsGenericEnumerableTests
	{
		sealed class Result : IResult<object>
		{
			public object Get() => throw new NotSupportedException();
		}

		sealed class Command : ICommand<None>
		{
			public void Execute(None parameter)
			{
				throw new NotSupportedException();
			}
		}

		sealed class Subject : IEnumerable<object>
		{
			public IEnumerator<object> GetEnumerator() => throw new NotSupportedException();

			IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
		}

		sealed class Subject<T> : IEnumerable<T>
		{
			public IEnumerator<T> GetEnumerator() => throw new NotSupportedException();

			IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
		}

		sealed class Integers : IEnumerable<int>
		{
			public IEnumerator<int> GetEnumerator() => throw new NotSupportedException();

			IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
		}

		[Fact]
		public void Are()
		{
			new[]
				{
					typeof(Subject),
					typeof(Subject<int>),
					typeof(Integers)
				}.All(ImplementsGenericEnumerable.Default.Get)
				 .Should()
				 .BeTrue();
		}

		[Fact]
		public void AreNot() => new[]
			{
				typeof(Result),
				typeof(Command)
			}.All(ImplementsGenericEnumerable.Default.Get)
			 .Should()
			 .BeFalse();
	}
}