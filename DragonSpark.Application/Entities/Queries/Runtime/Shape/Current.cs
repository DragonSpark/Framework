using DragonSpark.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DragonSpark.Application.Entities.Queries.Runtime.Shape
{
	public sealed class Current<T> : Collection<T>
	{
		public static Current<T> Default { get; } = new Current<T>();

		Current() : this(Empty.Array<T>(), null) {}

		public Current(IList<T> list, ulong? total) : base(list) => Total = total;

		public ulong? Total { get; }
	}
}