using System;
using System.Security.Claims;
using DragonSpark.Compose;
using DragonSpark.Text;

namespace DragonSpark.Application.Security.Identity.Bearer;

sealed class Bearer : Formatter<ClaimsIdentity>, IBearer
{
	public Bearer(BearerIdentity bearer, ISign sign) : base(bearer.Then().Select(sign)) {}
}

// TODO
public interface IMessageBearer : IFormatter<ClaimsIdentity>;
sealed class MessageBearer : Formatter<ClaimsIdentity>, IMessageBearer
{
	public MessageBearer(BearerIdentity bearer, IToken token, MessageBearerSettings settings)
		: base(bearer.Then().Select(x => new ClaimsSecurityDescriptorInput(x, settings.Expires)).Select(token)) {}
}

public sealed record MessageBearerSettings
{
	public TimeSpan Expires { get; set; } = TimeSpan.FromMinutes(1);
}