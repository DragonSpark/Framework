using Radzen;
using System;
using System.Collections.Generic;

namespace DragonSpark.Presentation.Components.Content.Sequences
{
	sealed class LoadDataArgsEqualityComparer : IEqualityComparer<LoadDataArgs>
	{
		public static LoadDataArgsEqualityComparer Default { get; } = new();

		LoadDataArgsEqualityComparer() : this(StringComparer.InvariantCultureIgnoreCase) {}

		readonly IEqualityComparer<string> _comparer;

		public LoadDataArgsEqualityComparer(IEqualityComparer<string> comparer) => _comparer = comparer;

		public bool Equals(LoadDataArgs? x, LoadDataArgs? y)
			=> ReferenceEquals(x, y) || !ReferenceEquals(x, null)
			   &&
			   !ReferenceEquals(y, null)
			   &&
			   x.GetType() == y.GetType()
			   &&
			   x.Skip == y.Skip && x.Top == y.Top
			   &&
			   _comparer.Equals(x.OrderBy, y.OrderBy)
			   &&
			   _comparer.Equals(x.Filter, y.Filter);

		public int GetHashCode(LoadDataArgs obj)
		{
			var code = new HashCode();
			code.Add(obj.Skip);
			code.Add(obj.Top);
			code.Add(obj.OrderBy, _comparer);
			code.Add(obj.Filter, _comparer);
			var result = code.ToHashCode();
			return result;
		}
	}
}