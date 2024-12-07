using DragonSpark.Model.Selection;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace DragonSpark.Application.Compose.Runtime;

sealed class CopyList<TList, T> : ISelect<CopyListInput<TList, T>, TList> where TList : List<T>
{
	public static CopyList<TList, T> Default { get; } = new();

	CopyList() {}

	public TList Get(CopyListInput<TList, T> parameter)
	{
		var (source, result) = parameter;
		result.Capacity      = result.Capacity <= source.Length ? result.Capacity : source.Length;
		var destination = CollectionsMarshal.AsSpan(result);
		source.Span.CopyTo(destination);
		return result;
	}
}