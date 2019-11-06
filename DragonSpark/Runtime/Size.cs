using JetBrains.Annotations;
using DragonSpark.Model.Results;
using DragonSpark.Reflection.Types;
using System;
using System.Runtime.CompilerServices;

namespace DragonSpark.Runtime
{
	public sealed class Size : ISize
	{
		public static Size Default { get; } = new Size();

		Size() : this(new Generic<uint>(typeof(SizeOf<>))) {}

		readonly IGeneric<uint> _generic;

		public Size(IGeneric<uint> generic) => _generic = generic;

		public uint Get(Type type) => _generic.Get(type).Invoke();

		sealed class SizeOf<T> : IResult<uint>
		{
			[UsedImplicitly]
			public static SizeOf<T> Instance { get; } = new SizeOf<T>();

			SizeOf() {}

			public uint Get() => (uint)Unsafe.SizeOf<T>();
		}
	}
}