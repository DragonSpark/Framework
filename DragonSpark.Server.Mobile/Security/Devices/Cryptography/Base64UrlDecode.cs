using System.Buffers.Text;
using DragonSpark.Compose;
using DragonSpark.Model.Sequences.Memory;
using JetBrains.Annotations;

namespace DragonSpark.Server.Mobile.Security.Devices.Cryptography;

sealed class Base64UrlDecode : ILease<ReadOnlyMemory<char>, byte>
{
    public static Base64UrlDecode Default { get; } = new();

    Base64UrlDecode() : this(NewLeasing<byte>.Default) {}

    readonly INewLeasing<byte> _leasing;

    public Base64UrlDecode(INewLeasing<byte> leasing) => _leasing = leasing;

    [MustDisposeResource]
    public Leasing<byte> Get(ReadOnlyMemory<char> parameter)
    {
        if (parameter.Length != 0)
        {
            var max   = Base64Url.GetMaxDecodedLength(parameter.Length);
            var lease = _leasing.Get(max);

            if (Base64Url.TryDecodeFromChars(parameter.Span, lease.Store, out var written))
            {
                return lease.Size(written);
            }

            lease.Dispose();
            throw new FormatException("Invalid base64url input.");
        }

        return Leasing<byte>.Default;
    }
}