using DragonSpark.Model.Selection.Stores;
using System.Net.Http;
using System.Security.Claims;

namespace DragonSpark.Application.Communication.Http;

public sealed class AuthenticatedClientReferences : ReferenceValueStore<ClaimsPrincipal, HttpClient>
{
	public AuthenticatedClientReferences(AuthenticatedClients source) : base(source) {}
}