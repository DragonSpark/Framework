using DragonSpark.Model.Selection;
using System.Collections.Generic;

namespace DragonSpark.Model.Sequences.Collections.Groups
{
	public interface IGroupPairs<T> : ISelect<IGroup<T>, Pair<GroupName, IList<T>>> {}
}