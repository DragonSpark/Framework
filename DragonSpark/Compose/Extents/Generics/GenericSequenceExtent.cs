using DragonSpark.Model.Sequences;
using System;
using System.Collections.Generic;

namespace DragonSpark.Compose.Extents.Generics;

public sealed class GenericSequenceExtent<T> : GenericExtent<IEnumerable<T>>
{
	readonly Type _definition;

	public GenericSequenceExtent(Type definition) : base(definition) => _definition = definition;

	public GenericExtent<T[]> Array() => new(_definition);

	public GenericExtent<Array<T>> Immutable() => new(_definition);
}

public sealed class GenericSequenceExtent<T1, T> : GenericExtent<IEnumerable<T>>
{
	readonly Type _definition;

	public GenericSequenceExtent(Type definition) : base(definition) => _definition = definition;

	public GenericExtent<T1, T[]> Array() => new(_definition);

	public GenericExtent<T1, Array<T>> Immutable() => new(_definition);
}

public sealed class GenericSequenceExtent<T1, T2, T> : GenericExtent<IEnumerable<T>>
{
	readonly Type _definition;

	public GenericSequenceExtent(Type definition) : base(definition) => _definition = definition;

	public GenericExtent<T1, T2, T[]> Array() => new(_definition);

	public GenericExtent<T1, T2, Array<T>> Immutable() => new(_definition);
}

public sealed class GenericSequenceExtent<T1, T2, T3, T> : GenericExtent<IEnumerable<T>>
{
	readonly Type _definition;

	public GenericSequenceExtent(Type definition) : base(definition) => _definition = definition;

	public GenericExtent<T1, T2, T3, T[]> Array() => new(_definition);

	public GenericExtent<T1, T2, T3, Array<T>> Immutable() => new(_definition);
}

public sealed class GenericSequenceExtent<T1, T2, T3, T4, T> : GenericExtent<IEnumerable<T>>
{
	readonly Type _definition;

	public GenericSequenceExtent(Type definition) : base(definition) => _definition = definition;

	public GenericExtent<T1, T2, T3, T4, T[]> Array() => new(_definition);

	public GenericExtent<T1, T2, T3, T4, Array<T>> Immutable() => new(_definition);
}