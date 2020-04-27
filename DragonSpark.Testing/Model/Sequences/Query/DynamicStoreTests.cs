using DragonSpark.Model.Sequences.Query;
using FluentAssertions;
using System.Linq;
using Xunit;

namespace DragonSpark.Testing.Model.Sequences.Query
{
	public sealed class DynamicStoreTests
	{
		[Fact]
		public void Verify()
		{
			var first  = new[] {1, 2, 3, 4};
			var second = new[] {5, 6, 7, 8};
			var third  = new[] {9, 10};

			new DynamicStore<int>(1024).Add(first)
			                           .Add(second)
			                           .Add(third)
			                           .Get()
			                           .Instance.Should()
			                           .Equal(first.Concat(second).Concat(third));
		}

		[Fact]
		public void VerifyLarge()
		{
			var first  = Enumerable.Range(0, 2048).ToArray();
			var second = Enumerable.Range(0, 2048).ToArray();
			var third  = Enumerable.Range(0, 4096).ToArray();
			var fourth = Enumerable.Range(0, 8192).ToArray();

			new DynamicStore<int>(1024)
				.Add(first)
				.Add(second)
				.Add(third)
				.Add(fourth)
				.Get()
				.Instance.Should()
				.Equal(first.Concat(second).Concat(third).Concat(fourth));
		}
	}
}