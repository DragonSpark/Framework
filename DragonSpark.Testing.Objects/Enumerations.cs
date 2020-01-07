using DragonSpark.Compose;
using DragonSpark.Model.Sequences;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Testing.Objects
{
	// ReSharper disable PossibleMultipleEnumeration

	public class Enumerations<T>
	{
		public Enumerations(uint count) : this(FixtureInstance.Default.Then().Select(new Many<T>(count)).Get().Get()) {}

		public Enumerations(IEnumerable<T> source)
			: this(source, Objects.Near.Default, Objects.Far.Default) {}

		public Enumerations(IEnumerable<T> source, Selection near, Selection far)
			: this(source,
			       source.Skip((int)near.Start).Take((int)near.Length.Instance),
			       source.Skip((int)far.Start).Take((int)far.Length.Instance)) {}

		public Enumerations(IEnumerable<T> full, IEnumerable<T> near, IEnumerable<T> far)
		{
			Full = full;
			Near = near;
			Far  = far;
		}

		public IEnumerable<T> Full { get; }
		public IEnumerable<T> Near { get; }
		public IEnumerable<T> Far { get; }

		public Enumerations<T> Get(Func<IEnumerable<T>, IEnumerable<T>> select)
			=> new Enumerations<T>(select(Full), select(Near), select(Far));
	}
}