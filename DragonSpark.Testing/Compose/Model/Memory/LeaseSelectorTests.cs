using BenchmarkDotNet.Attributes;
using DragonSpark.Compose;
using DragonSpark.Model.Sequences.Memory;
using FluentAssertions;
using NetFabric.Hyperlinq;
using System.Buffers;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

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

			var sut = first.AsValueEnumerable().AsLease().Then().Concat(second).Result().ToArray();
			sut.Should().Equal(first.Concat(second));
		}

		[Fact]
		public void VerifyMemoryOwner()
		{
			using var native = MemoryPool<int>.Shared.Rent(3);
			native.Memory.Length.Should().NotBe(3);

			using var sut = Leases<int>.Default.Get(3);

			var memory = sut.AsMemory();
			memory.Length.Should().Be(3);
			memory.Span.Length.Should().Be(3);
			sut.AsSpan().Length.Should().Be(3);
		}

		readonly ITestOutputHelper _output;

		public LeaseSelectorTests(ITestOutputHelper output) => _output = output;

		[Fact]
		public void VerifySum()
		{
			var first  = new[] { 1, 2, 3, 4 };
			var second = new[] { 5, 6, 7, 8 };

			var       result   = 0;
			using var other    = second.Hide().AsValueEnumerable().AsLease();
			var       memory = other.AsMemory();

			for (int i = 0; i < memory.Length; i++)
			{
				_output.WriteLine(memory.Span[i].ToString());
			}
			using var sut      = first.Hide().AsValueEnumerable().AsLease().Then().Concat(memory).Result();
			var       span     = sut.AsSpan();

			_output.WriteLine($"{memory.Length} - {span.Length}");


			for (var i = 0; i < sut.Length; i++)
			{
				var i1 = span[i];
				_output.WriteLine(i1.ToString());
				result += i1;
			}

			result.Should().Be(first.Concat(second).Sum());
		}

		[Fact]
		public void VerifySumEnumerable()
		{
			for (int j = 0; j < 100; j++)
			{
				var first    = new[] { 1, 2, 3, 4 };
				var second   = new[] { 5, 6, 7, 8 };
				var expected = first.Concat(second).Sum();
				using var sut = first.Hide()
				                     .AsValueEnumerable()
				                     .AsLease()
				                     .Then()
				                     .Concat(second.Hide().AsValueEnumerable())
				                     .Result();
				var span   = sut.AsSpan();
				var result = 0;

				for (var i = 0; i < sut.Length; i++)
				{
					result += span[i];
				}

				result.Should().Be(expected);
			}
		}

		public class Benchmarks
		{
			readonly int[] first = { 1, 2, 3, 4 }, second = { 5, 6, 7, 8 };

			[Benchmark(Baseline = true)]
			public int Allocations()
			{
				var result = 0;
				foreach (var element in first.Concat(second))
				{
					result += element;
				}

				return result;
			}

			/*[Benchmark]
			public int MeasureValue()
			{
				var result = 0;
				foreach (var element in first.Concat(second).AsValueEnumerable())
				{
					result += element;
				}

				return result;
			}*/

			[Benchmark]
			public int NoAllocations()
			{
				var       result = 0;
				using var sut    = first.AsValueEnumerable().AsLease().Then().Concat(second).Result();
				var       span   = sut.AsSpan();
				for (var i = 0; i < sut.Length; i++)
				{
					result += span[i];
				}

				return result;
			}

			/*[Benchmark]
			public int MeasureBoxing()
			{
				var       result = 0;
				foreach (var element in first.AsValueEnumerable().Concat(second).AsValueEnumerable())
				{
					result += element;
				}

				return result;
			}*/
		}
	}
}