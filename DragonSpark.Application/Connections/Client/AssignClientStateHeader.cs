using System.Net;

namespace DragonSpark.Application.Connections.Client;

sealed class AssignClientStateHeader : AssignHeader
{
	public AssignClientStateHeader(CurrentClientState state) : base(HttpRequestHeader.Cookie.ToString(), state) {}
}