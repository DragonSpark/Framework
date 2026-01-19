using System.Security.Claims;
using DragonSpark.Application.Security.Identity.Bearer;
using DragonSpark.Compose;
using DragonSpark.Text;

namespace DragonSpark.Application.AspNet.Security.Identity.Bearer;

sealed class MessageBearer : Formatter<ClaimsIdentity>, IMessageBearer
{
    public MessageBearer(BearerIdentity bearer, IToken token, MessageBearerSettings settings)
        : base(bearer.Then().Select(x => new ClaimsSecurityDescriptorInput(x, settings.Expires)).Select(token)) {}
}