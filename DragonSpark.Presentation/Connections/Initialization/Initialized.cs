using Microsoft.AspNetCore.Http;

namespace DragonSpark.Presentation.Connections.Initialization;

sealed class Initialized : IInitialized
{
	public static Initialized Default { get; } = new();

	Initialized() : this(ConnectionIdentifierName.Default) {}

	readonly string _key;

	public Initialized(string key) => _key = key;

	public bool Get(HttpContext parameter) => parameter.Request.Cookies.ContainsKey(_key);
}