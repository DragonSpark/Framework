using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Presentation.Connections.Initialization;

sealed class ClearIdentifier : ICommand<HttpContext>
{
	public static ClearIdentifier Default { get; } = new();

	ClearIdentifier() : this(ConnectionIdentifierName.Default) {}

	readonly string _name;

	public ClearIdentifier(string name) => _name = name;

	public void Execute(HttpContext parameter)
	{
		parameter.Response.Cookies.Delete(_name);
	}
}