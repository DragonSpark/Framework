using System.Security.Cryptography;
using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences.Memory;
using NetFabric.Hyperlinq;

namespace DragonSpark.Server.Mobile.Security.Devices.Cryptography;

sealed class JoseToDer : ISelect<ReadOnlyMemory<byte>, Lease<byte>>
{
    public static JoseToDer Default { get; } = new();

    JoseToDer() : this(IntegerToDer.Default, NewLeasing<byte>.Default) {}

    readonly ISelect<ReadOnlyMemory<byte>, Lease<byte>> _der;
    readonly INewLeasing<byte>                          _new;

    public JoseToDer(ISelect<ReadOnlyMemory<byte>, Lease<byte>> der, INewLeasing<byte> @new)
    {
        _der = der;
        _new = @new;
    }

    public Lease<byte> Get(ReadOnlyMemory<byte> parameter)
    {
        switch (parameter.Length)
        {
            case 64:
            {
                using var derR   = _der.Get(parameter[..32]);
                using var derS   = _der.Get(parameter.Slice(32, 32));
                var       result = _new.Get(2 + derR.Length + derS.Length);
                result.Store[0] = 0x30;
                result.Store[1] = (byte)(derR.Length + derS.Length);
                Buffer.BlockCopy(derR.Rented, 0, result.Store, 2, derR.Length);
                Buffer.BlockCopy(derS.Rented, 0, result.Store, 2 + derR.Length, derS.Length);
                return result.AsEnumerable();
            }
            default:
                throw new CryptographicException("Invalid JOSE ECDSA length");
        }
    }
}