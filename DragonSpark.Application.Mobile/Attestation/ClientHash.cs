using System;
using System.Security.Cryptography;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Results.Stop;

namespace DragonSpark.Application.Mobile.Attestation;

public sealed class ClientHash : StopAware<string>
{
    public ClientHash(IClientKey key)
        : base(key.Then()
                  .Select(Convert.FromBase64String)
                  .Select(SHA256.HashData)
                  .Select(Convert.ToBase64String)
                  .Out()) {}
}