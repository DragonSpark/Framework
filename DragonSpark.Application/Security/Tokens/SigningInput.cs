using System.Buffers;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Model.Sequences.Memory;

namespace DragonSpark.Application.Security.Tokens;

sealed class SigningInput : IStopAware<CreateProofInput, string>
{
    readonly IDeviceKeyProvider              _keys;
    readonly ILease<WriteHeaderInput, char>  _header;
    readonly ILease<WritePayloadInput, char> _payload;

    public SigningInput(IDeviceKeyProvider keys) : this(keys, WriteHeader.Default, WritePayload.Default) {}

    public SigningInput(IDeviceKeyProvider keys, ILease<WriteHeaderInput, char> header,
                        ILease<WritePayloadInput, char> payload)
    {
        _keys    = keys;
        _header  = header;
        _payload = payload;
    }

    public async ValueTask<string> Get(Stop<CreateProofInput> parameter)
    {
        var ((message, token), stop) = parameter;
        var (kty, crv, x, y, _)      = await _keys.Off(stop);
        var       buffer = new ArrayBufferWriter<byte>(256);
        using var header = _header.Get(new(new(kty, crv, x, y), buffer));
        buffer.Clear();
        using var payload = _payload.Get(new(message, token, buffer));
        var       total   = header.Length + 1 + payload.Length;
        using var lease   = NewLeasing<char>.Default.Get(total);
        var       to      = lease.AsSpan();
        header.AsSpan().CopyTo(to);
        var length = (int)header.Length;
        to[length] = '.';
        payload.AsSpan().CopyTo(to[(length + 1)..]);
        return new(to);
    }
}