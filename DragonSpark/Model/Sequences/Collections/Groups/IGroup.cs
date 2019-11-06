using System.Collections.Generic;

namespace DragonSpark.Model.Sequences.Collections.Groups
{
	public interface IGroup<T> : IList<T>
	{
		GroupName Name { get; }
	}
}