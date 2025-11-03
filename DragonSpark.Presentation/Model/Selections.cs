using DragonSpark.Model;
using System.Collections.Generic;

namespace DragonSpark.Presentation.Model;

public class Selections<T> : List<Option<T>>
{
	public Selections() {}

	public Selections(IEnumerable<Option<T>> collection) : base(collection) {}

	public IEnumerable<T> Selected { get; set; } = Empty.Enumerable<T>();
}