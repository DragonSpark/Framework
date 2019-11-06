using System.Collections.Generic;
using DragonSpark.Model.Selection;
using DragonSpark.Runtime;

namespace DragonSpark.Model.Sequences.Collections.Groups
{
	public interface IGroupPairs<T> : ISelect<IGroup<T>, Pair<GroupName, IList<T>>> {}
}