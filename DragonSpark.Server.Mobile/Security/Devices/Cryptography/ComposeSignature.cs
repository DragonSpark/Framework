using System.Buffers.Text;
using DragonSpark.Compose;
using DragonSpark.Model.Sequences.Memory;

namespace DragonSpark.Server.Mobile.Security.Devices.Cryptography;

sealed class ComposeSignature : ILease<ReadOnlyMemory<char>, byte>
{
    public static ComposeSignature Default { get; } = new();

    ComposeSignature() : this(NewLeasing<byte>.Default) {}

    readonly INewLeasing<byte> _leasing;

    public ComposeSignature(INewLeasing<byte> leasing) => _leasing = leasing;

    public Leasing<byte> Get(ReadOnlyMemory<char> parameter)
    {
        var length = Base64Url.GetMaxDecodedLength(parameter.Length);
        var result = _leasing.Get(length);
        if (Base64Url.TryDecodeFromChars(parameter.Span, result.Store, out var written))
        {
            return result.Size(written);
        }

        result.Dispose();
        throw new FormatException("Invalid base64url signature");
    }
}