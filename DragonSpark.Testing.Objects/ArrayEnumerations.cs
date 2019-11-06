using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using DragonSpark.Runtime.Activation;

namespace DragonSpark.Testing.Objects
{
	sealed class ArrayEnumerations<T> : Enumerations<T>, IActivateUsing<uint>, IActivateUsing<IEnumerable<T>>
	{
		public static ArrayEnumerations<T> Default { get; } = new ArrayEnumerations<T>();

		ArrayEnumerations() : this(10_000u) {}

		[UsedImplicitly]
		public ArrayEnumerations(uint count) :
			this(FixtureInstance.Default.ToSelect().Select(new Many<T>(count)).Get()) {}

		public ArrayEnumerations(IEnumerable<T> source) : base(source.ToArray()) {}
	}
}