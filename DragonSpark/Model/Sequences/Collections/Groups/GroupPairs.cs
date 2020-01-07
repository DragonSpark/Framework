using System.Collections.Generic;

namespace DragonSpark.Model.Sequences.Collections.Groups
{
	sealed class GroupPairs<T> : IGroupPairs<T>
	{
		public static GroupPairs<T> Default { get; } = new GroupPairs<T>();

		GroupPairs() {}

		public Pair<GroupName, IList<T>> Get(IGroup<T> parameter)
			=> Pairs.Create(parameter.Name, (IList<T>)parameter);
	}
}