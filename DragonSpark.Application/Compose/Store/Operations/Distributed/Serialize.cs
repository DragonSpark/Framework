using DragonSpark.Model.Selection;
using System.Text.Json;

namespace DragonSpark.Application.Compose.Store.Operations.Distributed;

sealed class Serialize<T> : ISelect<T?, string>
{
	public static Serialize<T> Default { get; } = new();

	Serialize() {}

	public string Get(T? parameter) => JsonSerializer.Serialize(parameter);
}