using DragonSpark.Configuration;
using DragonSpark.Sources.Parameterized;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace DragonSpark.Windows.FileSystem
{
	public sealed class ByteReader : ConfigurableParameterizedSource<string, ImmutableArray<byte>>
	{
		public static ByteReader Default { get; } = new ByteReader();
		ByteReader() : this( System.IO.File.ReadAllBytes ) {}

		[UsedImplicitly]
		public ByteReader( Func<string, IEnumerable<byte>> factory ) : base( factory.AsImmutable ) {}
	}
}
