using DragonSpark.Model;
using System.Collections.Generic;

namespace DragonSpark.Presentation.Model;

public class Selections<T> : List<SelectionListing<T>>
{
	public Selections() {}

	public Selections(IEnumerable<SelectionListing<T>> collection) : base(collection) {}

	public IEnumerable<T> Selected { get; set; } = Empty.Enumerable<T>();
}