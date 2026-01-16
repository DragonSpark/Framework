using System.Security.Claims;
using DragonSpark.Compose;
using DragonSpark.Text;

namespace DragonSpark.Application.Security.Identity.Bearer;

sealed class Bearer : Formatter<ClaimsIdentity>, IBearer
{
	public Bearer(BearerIdentity bearer, ISign sign) : base(bearer.Then().Select(sign)) {}
}