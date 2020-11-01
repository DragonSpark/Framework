using System.Collections.Generic;

namespace DragonSpark.Application.Runtime
{
	public class SelectedCollection<T> : List<T> where T : class
	{
		public SelectedCollection(IEnumerable<T> list) : base(list) {}

		public virtual T? Selected { get; set; }
	}
}
