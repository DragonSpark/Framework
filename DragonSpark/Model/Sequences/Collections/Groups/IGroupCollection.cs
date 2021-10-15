using DragonSpark.Model.Selection;
using System.Collections.Generic;

namespace DragonSpark.Model.Sequences.Collections.Groups;

public interface IGroupCollection<T> : ISelect<GroupName, IList<T>>
{
	IArray<T> Values { get; }
}