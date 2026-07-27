using DragonSpark.Model.Sequences.Memory;

namespace DragonSpark.Server.Mobile.Security.Devices.Authentication;

public readonly record struct ParsedJws(
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