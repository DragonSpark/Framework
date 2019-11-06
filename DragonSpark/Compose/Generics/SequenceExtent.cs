using System;
using System.Collections.Generic;
using DragonSpark.Model.Sequences;

namespace DragonSpark.Compose.Generics
{
	public sealed class SequenceExtent<T> : Extent<IEnumerable<T>>
	{
		readonly Type _definition;

		public SequenceExtent(Type definition) : base(definition) => _definition = definition;

		public Extent<T[]> Array() => new Extent<T[]>(_definition);

		public Extent<Array<T>> Immutable() => new Extent<Array<T>>(_definition);
	}

	public sealed class SequenceExtent<T1, T> : Extent<IEnumerable<T>>
	{
		readonly Type _definition;

		public SequenceExtent(Type definition) : base(definition) => _definition = definition;

		public Extent<T1, T[]> Array() => new Extent<T1, T[]>(_definition);

		public Extent<T1, Array<T>> Immutable() => new Extent<T1, Array<T>>(_definition);
	}

	public sealed class SequenceExtent<T1, T2, T> : Extent<IEnumerable<T>>
	{
		readonly Type _definition;

		public SequenceExtent(Type definition) : base(definition) => _definition = definition;

		public Extent<T1, T2, T[]> Array() => new Extent<T1, T2, T[]>(_definition);

		public Extent<T1, T2, Array<T>> Immutable() => new Extent<T1, T2, Array<T>>(_definition);
	}

	public sealed class SequenceExtent<T1, T2, T3, T> : Extent<IEnumerable<T>>
	{
		readonly Type _definition;

		public SequenceExtent(Type definition) : base(definition) => _definition = definition;

		public Extent<T1, T2, T3, T[]> Array() => new Extent<T1, T2, T3, T[]>(_definition);

		public Extent<T1, T2, T3, Array<T>> Immutable() => new Extent<T1, T2, T3, Array<T>>(_definition);
	}

	public sealed class SequenceExtent<T1, T2, T3, T4, T> : Extent<IEnumerable<T>>
	{
		readonly Type _definition;

		public SequenceExtent(Type definition) : base(definition) => _definition = definition;

		public Extent<T1, T2, T3, T4, T[]> Array() => new Extent<T1, T2, T3, T4, T[]>(_definition);

		public Extent<T1, T2, T3, T4, Array<T>> Immutable() => new Extent<T1, T2, T3, T4, Array<T>>(_definition);
	}
}