using DragonSpark.Model.Selection;
using NetFabric.Hyperlinq;
using Syncfusion.Blazor.Data;
using System.Buffers;
using System.Collections;
using System.Linq;

namespace DragonSpark.SyncfusionRendering.Queries;

sealed class PerformSelect<T> : ISelect<PerformSelectInput<T>, IQueryable<T>>
{
	public static PerformSelect<T> Default { get; } = new();

	PerformSelect() {}

	public IQueryable<T> Get(PerformSelectInput<T> parameter)
	{
		var (source, columns) = parameter;
		if (source.GetObjectType() == typeof(object))
		{
			IEnumerator enumerator = source.GetEnumerator();
			if (enumerator.MoveNext())
			{
				// ReSharper disable once ReturnValueOfPureMethodIsNotUsed
				enumerator.Current?.GetType();
			}
		}

		using var lease = columns.AsValueEnumerable().Where(x => x != null!).ToArray(ArrayPool<string>.Shared);
		var       names = string.Join(",", lease);
		return source.Select<T>(names).Cast<T>().Distinct();
	}
}