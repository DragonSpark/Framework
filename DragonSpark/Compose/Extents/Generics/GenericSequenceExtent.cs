using DragonSpark.Model.Sequences;
using System;
using System.Collections.Generic;

namespace DragonSpark.Compose.Extents.Generics
{
	public sealed class GenericSequenceExtent<T> : GenericExtent<IEnumerable<T>>
	{
		readonly Type _definition;

		public GenericSequenceExtent(Type definition) : base(definition) => _definition = definition;

		public GenericExtent<T[]> Array() => new GenericExtent<T[]>(_definition);

		public GenericExtent<Array<T>> Immutable() => new GenericExtent<Array<T>>(_definition);
	}

	public sealed class GenericSequenceExtent<T1, T> : GenericExtent<IEnumerable<T>>
	{
		readonly Type _definition;

		public GenericSequenceExtent(Type definition) : base(definition) => _definition = definition;

		public GenericExtent<T1, T[]> Array() => new GenericExtent<T1, T[]>(_definition);

		public GenericExtent<T1, Array<T>> Immutable() => new GenericExtent<T1, Array<T>>(_definition);
	}

	public sealed class GenericSequenceExtent<T1, T2, T> : GenericExtent<IEnumerable<T>>
	{
		readonly Type _definition;

		public GenericSequenceExtent(Type definition) : base(definition) => _definition = definition;

		public GenericExtent<T1, T2, T[]> Array() => new GenericExtent<T1, T2, T[]>(_definition);

		public GenericExtent<T1, T2, Array<T>> Immutable() => new GenericExtent<T1, T2, Array<T>>(_definition);
	}

	public sealed class GenericSequenceExtent<T1, T2, T3, T> : GenericExtent<IEnumerable<T>>
	{
		readonly Type _definition;

		public GenericSequenceExtent(Type definition) : base(definition) => _definition = definition;

		public GenericExtent<T1, T2, T3, T[]> Array() => new GenericExtent<T1, T2, T3, T[]>(_definition);

		public GenericExtent<T1, T2, T3, Array<T>> Immutable() => new GenericExtent<T1, T2, T3, Array<T>>(_definition);
	}

	public sealed class GenericSequenceExtent<T1, T2, T3, T4, T> : GenericExtent<IEnumerable<T>>
	{
		readonly Type _definition;

		public GenericSequenceExtent(Type definition) : base(definition) => _definition = definition;

		public GenericExtent<T1, T2, T3, T4, T[]> Array() => new GenericExtent<T1, T2, T3, T4, T[]>(_definition);

		public GenericExtent<T1, T2, T3, T4, Array<T>> Immutable()
			=> new GenericExtent<T1, T2, T3, T4, Array<T>>(_definition);
	}
}