using System;
using DragonSpark.Model.Sequences.Memory;

namespace DragonSpark.Server.Mobile.Security.Devices;

public readonly record struct JwsResult(
    Leasing<byte> Header,
    Leasing<byte> Payload,
    ReadOnlyMemory<char> SigningInput,
    Leasing<byte> RawSignature) : IDisposable
{
    public void Dispose()
    {
        Header.Dispose();
        Payload.Dispose();
        RawSignature.Dispose();
    }
}