using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DragonSpark.Application.Runtime
{
	public class SelectedCollection<T> : List<T> where T : class
	{
		public SelectedCollection(IEnumerable<T> list) : base(list) {}

		[Required]
		public T? Selected { get; set; }
	}
}
