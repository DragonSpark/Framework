using BenchmarkDotNet.Attributes;
using DragonSpark.Compose;
using DragonSpark.Testing.Objects;
using System;
using System.Linq;

namespace DragonSpark.Testing.Application
{
	public class EnumerationBenchmarks
	{
		readonly Func<uint, Enumerations<uint>> _classics;
		readonly Sequencing<uint>               _subject;

		Enumerations<uint> _classic;

		uint _count;

		uint[] _source;

		public EnumerationBenchmarks() : this(Numbers.Default
		                                             .Select(x => x.Open().Select(y => (uint)y))
		                                             .Select(x => new ArrayEnumerations<uint>(x))
		                                             .Get,
		                                      Sequencing<uint>.Default) {}

		public EnumerationBenchmarks(Func<uint, Enumerations<uint>> classics, Sequencing<uint> subject)
		{
			_classics = classics;
			_subject  = subject;
		}

		[Params(1u, 2u, 3u, 4u, 5u, 8u, 16u, 32u, 64u, 128u, 256u, 512u, 1024u, 1025u, 2048u, 4096u, 8196u,
			10_000u, 100_000u, 1_000_000u, 10_000_000u, 100_000_000u)]
		public uint Count
		{
			get => _count;
			set
			{
				_classic = _classics(_count = value);
				_source  = _classic.Full.ToArray();
			}
		}

		[Benchmark]
		public Array Full() => _subject.Full.Get(_source);

		[Benchmark]
		public Array FullClassic() => _classic.Full.ToArray();
	}
}