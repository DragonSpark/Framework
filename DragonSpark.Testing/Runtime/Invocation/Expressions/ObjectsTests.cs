using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Sequences;
using FluentAssertions;
using Xunit;

// ReSharper disable All

namespace DragonSpark.Testing.Runtime.Invocation.Expressions
{
	public class ObjectsTests
	{
		[Fact]
		public void Execute()
		{
			var count   = 0;
			var command = new Command<(int, int)>(tuple => count = tuple.Item1 + tuple.Item2);
			command.Execute(1, 2);
			count.Should()
			     .Be(3);
		}

		[Fact]
		public void ExecuteItems()
		{
			var count   = 0u;
			var command = new Command<Array<int>>(items => count = items.Length);
			command.Execute(1, 2, 3, 4, 5, 6);
			count.Should()
			     .Be(6);
		}

		[Fact]
		public void ExecuteTriple()
		{
			var count   = 0;
			var command = new Command<(int, int, int)>(tuple => count = tuple.Item1 + tuple.Item2 + tuple.Item3);
			command.Execute(1, 2, 3);
			count.Should()
			     .Be(6);
		}
	}
}