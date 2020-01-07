using DragonSpark.Compose;
using DragonSpark.Runtime.Activation;
using JetBrains.Annotations;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Testing.Objects
{
	sealed class ArrayEnumerations<T> : Enumerations<T>, IActivateUsing<uint>, IActivateUsing<IEnumerable<T>>
	{
		public static ArrayEnumerations<T> Default { get; } = new ArrayEnumerations<T>();

		ArrayEnumerations() : this(10_000u) {}

		[UsedImplicitly]
		public ArrayEnumerations(uint count)
			: this(FixtureInstance.Default.Then().Select(new Many<T>(count)).Get().Get()) {}

		public ArrayEnumerations(IEnumerable<T> source) : base(source.ToArray()) {}
	}
}