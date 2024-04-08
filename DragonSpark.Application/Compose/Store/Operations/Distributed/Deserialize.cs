using DragonSpark.Model.Selection;
using System.Text.Json;

namespace DragonSpark.Application.Compose.Store.Operations.Distributed;

sealed class Deserialize<T> : ISelect<string, T?>
{
	public static Deserialize<T> Default { get; } = new();

	Deserialize() {}

	public T? Get(string parameter) => JsonSerializer.Deserialize<T?>(parameter);
}