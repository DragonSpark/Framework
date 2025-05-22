using System;
using System.Text.Json.Serialization;

namespace DragonSpark.Application.Communication.Http.Security;

[method: JsonConstructor]
public sealed record ChallengeRequest(string Address, Guid Identifier)
{
    public ChallengeRequest(string Address) : this(Address, Guid.NewGuid()) {}
}