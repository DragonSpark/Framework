using DragonSpark.Model;
using System.Collections.Generic;

namespace DragonSpark.Presentation.Model
{
	public class Selections<T> : List<SelectionListing<T>>
	{
		public IEnumerable<T> Selected { get; set; } = Empty.Enumerable<T>();
	}
}