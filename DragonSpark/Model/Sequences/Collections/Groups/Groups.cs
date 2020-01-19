using DragonSpark.Compose;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Model.Sequences.Collections.Groups
{
	class Groups<T> : IArray<IGroup<T>>
	{
		readonly Func<GroupName, IGroup<T>> _factory;
		readonly Array<GroupName>           _phases;

		public Groups(IEnumerable<GroupName> phases) : this(phases, x => new Group<T>(x)) {}

		public Groups(IEnumerable<GroupName> phases, Func<GroupName, IGroup<T>> factory)
			: this(phases.Result(), factory) {}

		public Groups(Array<GroupName> phases, Func<GroupName, IGroup<T>> factory)
		{
			_phases  = phases;
			_factory = factory;
		}

		public Array<IGroup<T>> Get() => _phases.Open().Select(_factory).ToArray();
	}
}