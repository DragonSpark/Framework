using DragonSpark.Compose;
using DragonSpark.Text;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Bearer;

sealed class Bearer : Formatter<ClaimsIdentity>, IBearer
{
	public Bearer(ISign sign, DetermineBearerIdentity bearer) : base(bearer.Then().Select(sign)) {}
}