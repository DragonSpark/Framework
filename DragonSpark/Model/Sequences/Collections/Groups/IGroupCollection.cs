using System.Collections.Generic;
using DragonSpark.Model.Selection;

namespace DragonSpark.Model.Sequences.Collections.Groups
{
	public interface IGroupCollection<T> : ISelect<GroupName, IList<T>>
	{
		IArray<T> Values { get; }
	}
}