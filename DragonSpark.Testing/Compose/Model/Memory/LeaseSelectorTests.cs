using DragonSpark.Compose;
using FluentAssertions;
using NetFabric.Hyperlinq;
using System.Linq;
using Xunit;

namespace DragonSpark.Testing.Compose.Model.Memory
{
	public sealed class LeaseSelectorTests
	{
		[Fact]
		public void Verify()
		{
			var instances = new BaseType[]
			{
				new(), new Extended(), new Extended(), new(), new()
			};

			var sut = instances.AsValueEnumerable().AsLease().Then().OfType<Extended>().ToArray();

			sut.Should().HaveCount(2).And.AllBeOfType<Extended>();
			sut.Should().Equal(instances.OfType<Extended>());
		}

		class BaseType {}

		sealed class Extended : BaseType {}

		[Fact]
		public void VerifyConcat()
		{
			var first  = new[] { 1, 2, 3, 4 };
			var second = new[] { 5, 6, 7, 8 };

			var sut = first.AsValueEnumerable().AsLease().Then().Concat(second).ToArray();
			sut.Should().Equal(first.Concat(second));
		}
	}
}