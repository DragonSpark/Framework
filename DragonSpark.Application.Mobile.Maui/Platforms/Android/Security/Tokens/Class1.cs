using System;
using System.Threading;
using System.Threading.Tasks;
using Android.Security.Keystore;
using DragonSpark.Application.Security.Tokens;
using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Model.Sequences;
using DragonSpark.Text;
using Java.Math;
using Java.Security;
using Java.Security.Interfaces;
using Java.Security.Spec;
using Microsoft.Extensions.DependencyInjection;
using ECPoint = Java.Security.Spec.ECPoint;
using Signature = Java.Security.Signature;

namespace DragonSpark.Application.Mobile.Maui.Platforms.Android.Security.Tokens;

sealed class Registrations : Commands<IServiceCollection>
{
    public static Registrations Default { get; } = new();

    Registrations()
        : base(Application.Security.Tokens.Registrations.Default, LocalRegistrations.Default) {}
}

sealed class LocalRegistrations : ICommand<IServiceCollection>
{
    public static LocalRegistrations Default { get; } = new();

    LocalRegistrations() {}

    public void Execute(IServiceCollection parameter)
    {
        parameter.Start<IDeviceKeyProvider>().Forward<DeviceKeyProvider>().Singleton();
    }
}

sealed class DeterminePoint : IResult<ECPoint>
{
    public static DeterminePoint Default { get; } = new();

    DeterminePoint() : this(GetKeyStore.Default, GeneratorAwareLoadKeyPair.Default) {}

    readonly IResult<KeyStore> _store;
    readonly ILoadKeyPair      _load;

    public DeterminePoint(IResult<KeyStore> store, ILoadKeyPair load)
    {
        _store = store;
        _load  = load;
    }

    public ECPoint Get()
    {
        var store = _store.Get();
        var pair  = _load.Get(store);
        return pair.Public.Verify().To<IECPublicKey>().GetW().Verify();
    }
}

sealed class GetKeyStore : IResult<KeyStore>
{
    public static GetKeyStore Default { get; } = new();

    GetKeyStore() : this(KeyStoreName.Default) {}

    readonly string _key;

    public GetKeyStore(string key) => _key = key;

    public KeyStore Get()
    {
        var result = KeyStore.GetInstance(_key).Verify();
        result.Load(null);
        return result;
    }
}

sealed class KeyStoreName : Text.Text
{
    public static KeyStoreName Default { get; } = new();

    KeyStoreName() : base("AndroidKeyStore") {}
}

sealed class StoreAlias : Text.Text
{
    public static StoreAlias Default { get; } = new();

    StoreAlias() : base("dpop-device-key") {}
}

sealed class GenerateKeyPair : IResult<KeyPair>
{
    public static GenerateKeyPair Default { get; } = new();

    GenerateKeyPair() : this(StoreAlias.Default, KeyStoreName.Default, new ECGenParameterSpec("secp256r1")) {}

    readonly string                  _alias, _key;
    readonly IAlgorithmParameterSpec _parameter;

    public GenerateKeyPair(string alias, string key, IAlgorithmParameterSpec parameter)
    {
        _alias     = alias;
        _key       = key;
        _parameter = parameter;
    }

    public KeyPair Get()
    {
        var generator = KeyPairGenerator.GetInstance(KeyProperties.KeyAlgorithmEc, _key).Verify();

        var parameter = new KeyGenParameterSpec.Builder(_alias, KeyStorePurpose.Sign)
                        .SetAlgorithmParameterSpec(_parameter)
                        .Verify()
                        .SetDigests(KeyProperties.DigestNone)
                        .SetUserAuthenticationRequired(false)
                        .SetKeySize(256)
                        .Build();

        generator.Initialize(parameter);
        return generator.GenerateKeyPair().Verify();
    }
}

sealed class GeneratorAwareLoadKeyPair : ILoadKeyPair
{
    public static GeneratorAwareLoadKeyPair Default { get; } = new();

    GeneratorAwareLoadKeyPair() : this(StoreAlias.Default, LoadKeyPair.Default, GenerateKeyPair.Default) {}

    readonly string           _alias;
    readonly ILoadKeyPair     _previous;
    readonly IResult<KeyPair> _generate;

    public GeneratorAwareLoadKeyPair(string alias, ILoadKeyPair previous, IResult<KeyPair> generate)
    {
        _alias    = alias;
        _previous = previous;
        _generate = generate;
    }

    public KeyPair Get(KeyStore parameter)
    {
        return parameter.ContainsAlias(_alias) ? _previous.Get(parameter) : _generate.Get();
    }
}

public interface ILoadKeyPair : ISelect<KeyStore, KeyPair>;

sealed class LoadKeyPair : ILoadKeyPair
{
    public static LoadKeyPair Default { get; } = new();

    LoadKeyPair() : this("dpop-device-key") {}

    readonly string _alias;

    public LoadKeyPair(string alias)
    {
        _alias = alias;
    }

    public KeyPair Get(KeyStore parameter)
    {
        return new KeyPair(parameter.GetCertificate(_alias).Verify().PublicKey,
                           parameter.GetKey(_alias, null)?.To<IPrivateKey>());
    }
}

sealed class DeterminePoints : ISelect<ECPoint, Points>
{
    public static DeterminePoints Default { get; } = new();

    DeterminePoints() : this(DeterminePoint.Default, FormatPoint.Default) {}

    readonly IResult<ECPoint>       _point;
    readonly IFormatter<BigInteger> _format;

    public DeterminePoints(IResult<ECPoint> point, IFormatter<BigInteger> format)
    {
        _point  = point;
        _format = format;
    }

    public Points Get(ECPoint parameter)
    {
        var point = _point.Get();
        var x     = _format.Get(point.AffineX.Verify());
        var y     = _format.Get(point.AffineY.Verify());
        return new(x, y);
    }
}

sealed class UnsignedBytes : IAlteration<Array<byte>>
{
    public static UnsignedBytes Default { get; } = new();

    UnsignedBytes() {}

    public Array<byte> Get(Array<byte> parameter)
    {
        var bytes = parameter.Open();
        return bytes.Length > 1 && bytes[0] == 0x00 ? bytes[1..] : bytes;
    }
}

sealed class FormatPoint : IFormatter<BigInteger>
{
    public static FormatPoint Default { get; } = new();

    FormatPoint() : this(UnsignedBytes.Default, TokenDataFormatter.Default) {}

    readonly IAlteration<Array<byte>> _unsigned;
    readonly IFormatter<Array<byte>>  _formatter;

    public FormatPoint(IAlteration<Array<byte>> unsigned, IFormatter<Array<byte>> formatter)
    {
        _unsigned  = unsigned;
        _formatter = formatter;
    }

    public string Get(BigInteger parameter)
    {
        var input    = parameter.ToByteArray().Verify();
        var unsigned = _unsigned.Get(input);
        var result   = _formatter.Get(unsigned);
        return result;
    }
}

public readonly record struct Points(string X, string Y);

sealed class DeviceKeyProvider : IDeviceKeyProvider
{
    public static DeviceKeyProvider Default { get; } = new();

    DeviceKeyProvider()
        : this(DeterminePoint.Default.Then().Select(DeterminePoints.Default).Get(), ComputeJkt.Default) {}

    readonly IResult<Points>    _points;
    readonly IFormatter<Points> _jkt;

    public DeviceKeyProvider(IResult<Points> points, IFormatter<Points> jkt)
    {
        _points = points;
        _jkt    = jkt;
    }

    public ValueTask<PublicJWK> Get(CancellationToken ct)
    {
        var points = _points.Get();
        var (x, y) = points;
        var jkt = _jkt.Get(points);
        return new PublicJWK("EC", "P-256", x, y, jkt).ToOperation();
    }
}

sealed class ComputeJkt : IFormatter<Points>
{
    public static ComputeJkt Default { get; } = new();

    ComputeJkt() : this(EncodedHashedText.Default, TokenDataFormatter.Default) {}

    readonly ISelect<string, byte[]> _encoded;
    readonly IFormatter<Array<byte>> _format;

    public ComputeJkt(ISelect<string, byte[]> encoded, IFormatter<Array<byte>> format)
    {
        _encoded = encoded;
        _format  = format;
    }

    public string Get(Points parameter)
    {
        var (x, y) = parameter;
        var json   = $$"""{"crv":"P-256","kty":"EC","x":"{{x}}","y":"{{y}}"}""";
        var hash   = _encoded.Get(json);
        var result = _format.Get(hash);
        return result;
    }
}

sealed class DeviceSigner : IDeviceSigner
{
    public static DeviceSigner Default { get; } = new();

    DeviceSigner() : this(GetKeyStore.Default, StoreAlias.Default, "NONEwithECDSA") {}

    readonly IResult<KeyStore> _store;
    readonly string            _alias;
    readonly string            _type;

    public DeviceSigner(IResult<KeyStore> store, string alias, string type)
    {
        _store = store;
        _alias = alias;
        _type  = type;
    }

    public ValueTask<ReadOnlyMemory<byte>> Get(Stop<ReadOnlyMemory<byte>> parameter)
    {
        var (digest, _) = parameter;

        var store     = _store.Get();
        var key       = (IPrivateKey)store.GetKey(_alias, null).Verify();
        var signature = Signature.GetInstance(_type).Verify();
        signature.InitSign(key);
        signature.Update(digest.ToArray());

        return signature.Sign().AsMemory().AsReadOnly().ToOperation();
    }
}