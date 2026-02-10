using DragonSpark.Application.Security.Tokens;
using DragonSpark.Compose;
using DragonSpark.Model.Results;
using DragonSpark.Model.Sequences.Memory;
using DragonSpark.Server.Mobile.Security.Devices.Registry;

namespace DragonSpark.Server.Mobile.Security.Devices.Validation;

sealed class DetermineRequestPublicKeyHash : IResult<Leasing<byte>>
{
    readonly ComposeRequestJwkHeader _header;
    readonly ILease<Points, byte>    _hash;

    public DetermineRequestPublicKeyHash(ComposeRequestJwkHeader header) : this(header, ComputePublicKeyHash.Default) {}

    public DetermineRequestPublicKeyHash(ComposeRequestJwkHeader header, ILease<Points, byte> hash)
    {
        _header = header;
        _hash   = hash;
    }

    public Leasing<byte> Get()
    {
        var (_, _, x, y) = _header.Get().Verify();
        return _hash.Get(new(x, y));
    }
}