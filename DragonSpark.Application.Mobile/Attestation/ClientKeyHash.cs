using System;
using System.Security.Cryptography;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Results.Stop;

namespace DragonSpark.Application.Mobile.Attestation;

sealed class ClientKeyHash : StopAware<string>, IClientKeyHash
{
    public ClientKeyHash(IClientKey key)
        : base(key.Then()
                  .Select(Convert.FromBase64String)
                  .Select(SHA256.HashData)
                  .Select(Convert.ToBase64String)
                  .Out()) {}
}