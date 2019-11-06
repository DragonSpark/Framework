using JetBrains.Annotations;
using DragonSpark.Model.Results;
using DragonSpark.Runtime;
using System.Collections.Immutable;

namespace DragonSpark.Model.Sequences
{
	public readonly struct Array<T> : IResult<ImmutableArray<T>>
	{
		public static implicit operator ImmutableArray<T>(Array<T> source) => source.Get();

		public static implicit operator Array<T>(T[] source) => new Array<T>(source);

		public static implicit operator T[](Array<T> source) => source.Open();

		public static Array<T> Empty { get; } = new Array<T>(Empty<T>.Array);

		readonly T[] _reference;

		public Array(params T[] elements) : this(elements, (uint)elements.Length) {}

		public Array(T[] reference, uint length)
		{
			_reference = reference;
			Length     = length;
		}

		public uint Length { get; }

		public ref readonly T this[uint index] => ref _reference[index];

		public ref readonly T this[int index] => ref _reference[index];

		[Pure]
		public T[] Copy() => Arrays<T>.Default.Get(_reference);

		[Pure]
		public ImmutableArray<T> Get() => ImmutableArray.Create(_reference);

		[Pure]
		public T[] Open() => _reference;
	}
}