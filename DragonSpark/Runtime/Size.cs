using DragonSpark.Compose;
using DragonSpark.Model.Results;
using DragonSpark.Reflection.Types;
using JetBrains.Annotations;
using System;
using System.Runtime.CompilerServices;

namespace DragonSpark.Runtime;

public sealed class Size : ISize
{
	public static Size Default { get; } = new Size();

	Size() : this(new Generic<uint>(typeof(SizeOf<>))) {}

	readonly IGeneric<uint> _generic;

	public Size(IGeneric<uint> generic) => _generic = generic;

	public uint Get(Type type) => _generic.Get(type)();

	sealed class SizeOf<T> : IResult<uint>
	{
		[UsedImplicitly]
		public static SizeOf<T> Instance { get; } = new SizeOf<T>();

		SizeOf() {}

		public uint Get() => (uint)Unsafe.SizeOf<T>();
	}
}