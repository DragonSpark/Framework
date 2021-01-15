using System;
using System.Collections.Generic;

namespace DragonSpark.Presentation.Components
{
	sealed class Adjustment<T> : IDisposable
	{
		readonly ICollection<IAdjust<T>> _source;
		readonly IAdjust<T>              _element;

		public Adjustment(ICollection<IAdjust<T>> source, IAdjust<T> element)
		{
			_source       = source;
			_element = element;
		}

		public void Dispose()
		{
			_source.Remove(_element);
		}
	}
}