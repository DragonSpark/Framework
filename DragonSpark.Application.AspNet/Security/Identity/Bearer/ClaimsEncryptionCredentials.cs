using System;
using DragonSpark.Application.Security.Identity.Bearer;
using DragonSpark.Model.Results;
using DragonSpark.Model.Sequences;
using Microsoft.IdentityModel.Tokens;

namespace DragonSpark.Application.AspNet.Security.Identity.Bearer;

sealed class ClaimsEncryptionCredentials : Instance<EncryptingCredentials>
{
    public ClaimsEncryptionCredentials(BearerSettings settings) : this(Convert.FromBase64String(settings.Key)) {}

    public ClaimsEncryptionCredentials(Array<byte> key)
        : this(new(key),
               key.Length switch
               {
                   32 => SecurityAlgorithms.Aes256KeyWrap,
                   64 => "dir",
                   _ => throw new
                            InvalidOperationException("Key must be 32 bytes (A256KW + A256CBC-HS512) or 64 bytes (dir + A256CBC-HS512).")
               },
               SecurityAlgorithms.Aes256CbcHmacSha512) {}

    public ClaimsEncryptionCredentials(SymmetricSecurityKey key, string algorithm, string encryption)
        : base(new(key, algorithm, encryption)) {}
}