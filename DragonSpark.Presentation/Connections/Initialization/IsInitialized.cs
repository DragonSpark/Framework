using Microsoft.AspNetCore.Http;

namespace DragonSpark.Presentation.Connections.Initialization;

sealed class IsInitialized : IIsInitialized
{
	public static IsInitialized Default { get; } = new();

	IsInitialized() : this(ConnectionIdentifierName.Default) {}

	readonly string _key;

	public IsInitialized(string key) => _key = key;

	public bool Get(HttpContext parameter) => parameter.Request.Cookies.ContainsKey(_key);
}