using System;
using System.Buffers;
using System.Buffers.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using DragonSpark.Application.Security;
using DragonSpark.Application.Security.Tokens;
using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Results.Stop;
using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using DragonSpark.Model.Sequences.Memory;
using NetFabric.Hyperlinq;

namespace DragonSpark.Application.Mobile.Security.Tokens;

class Class1 {}

public readonly record struct PublicJWK(string Kty, string Crv, string X, string Y, string Jkt);

public interface IDeviceKeyProvider : IStopAware<PublicJWK>;

public interface IDeviceSigner : IAltering<ReadOnlyMemory<byte>>;

public readonly record struct CreateProofInput(HttpRequestMessage Message, string? Token);

public readonly record struct DPoPHeader(string Kty, string Crv, string X, string Y);

public readonly record struct WriteHeaderInput(
    DPoPHeader Subject,
    Utf8JsonWriter Writer,
    ArrayBufferWriter<byte> Buffer);

sealed class WriteHeader : ILease<WriteHeaderInput, char>
{
    public static WriteHeader Default { get; } = new();

    WriteHeader() : this(MemoryTokenFormatter.Default, Base64UrlEncoder.Default) {}

    readonly ILease<ReadOnlyMemory<char>, char> _formatter;
    readonly ILease<ReadOnlyMemory<byte>, char> _encode;

    public WriteHeader(ILease<ReadOnlyMemory<char>, char> formatter, ILease<ReadOnlyMemory<byte>, char> encode)
    {
        _formatter = formatter;
        _encode    = encode;
    }

    public Leasing<char> Get(WriteHeaderInput parameter)
    {
        var ((kty, crv, x, y), writer, buffer) = parameter;
        writer.WriteString("typ", "dpop+jwt");
        writer.WriteString("alg", "ES256");
        writer.WritePropertyName("jwk");
        writer.WriteStartObject();
        writer.WriteString("kty", kty);
        writer.WriteString("crv", crv);
        writer.WriteString("x", x);
        writer.WriteString("y", y);
        writer.WriteEndObject();

        using var start  = _encode.Get(buffer.WrittenMemory);
        var       result = _formatter.Get(start.AsMemory());
        return result;
    }
}

public sealed class Base64UrlEncoder : ILease<ReadOnlyMemory<byte>, char>
{
    public static Base64UrlEncoder Default { get; } = new();

    Base64UrlEncoder() : this(NewLeasing<char>.Default) {}

    readonly INewLeasing<char> _leasing;

    public Base64UrlEncoder(INewLeasing<char> leasing) => _leasing = leasing;

    public Leasing<char> Get(ReadOnlyMemory<byte> parameter)
    {
        var from   = parameter.Span;
        var length = (from.Length + 2) / 3 * 4;

        // Lease char buffer
        var lease = _leasing.Get((uint)length);
        var to    = lease.AsSpan();

        // Encode into a temporary byte buffer
        Span<byte> temp = stackalloc byte[length];
        Base64.EncodeToUtf8(from, temp, out _, out var written);

        // Convert bytes → chars
        for (var i = 0; i < written; i++)
        {
            to[i] = (char)temp[i];
        }

        // URL-safe replacements
        to.Replace('+', '-');
        to.Replace('/', '_');

        // Trim '=' padding
        var trimmed = written;
        while (trimmed > 0 && to[trimmed - 1] == '=')
        {
            trimmed--;
        }

        return lease.Size(trimmed);
    }
}

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
        var             buffer = new ArrayBufferWriter<byte>(256);
        await using var writer = new Utf8JsonWriter(buffer);
        using var       header = _header.Get(new(new(kty, crv, x, y), writer, buffer));
        buffer.Clear();
        using var payload = _payload.Get(new(message, token, writer, buffer));

        var       total  = header.Length + 1 + payload.Length;
        using var result = NewLeasing<char>.Default.Get(total); // or inject INewLeasing<char> if preferred
        var       span   = result.AsSpan();
        header.AsSpan().CopyTo(span);
        var length = (int)header.Length;
        span[length] = '.';
        payload.AsSpan().CopyTo(span[(length + 1)..]);
        return new(span);
    }
}

public readonly record struct WritePayloadInput(
    HttpRequestMessage Message,
    string? Token,
    Utf8JsonWriter Writer,
    ArrayBufferWriter<byte> Buffer);

sealed class WritePayload : ILease<WritePayloadInput, char>
{
    public static WritePayload Default { get; } = new();

    WritePayload() : this(MemoryTokenFormatter.Default, Base64UrlEncoder.Default) {}

    readonly ILease<ReadOnlyMemory<char>, char> _formatter;
    readonly ILease<ReadOnlyMemory<byte>, char> _encode;

    public WritePayload(ILease<ReadOnlyMemory<char>, char> formatter, ILease<ReadOnlyMemory<byte>, char> encode)
    {
        _formatter = formatter;
        _encode    = encode;
    }

    public Leasing<char> Get(WritePayloadInput parameter)
    {
        var (message, token, writer, buffer) = parameter;

        writer.WriteString("htm", message.Method.Method);
        writer.WriteString("htu", message.RequestUri!.GetLeftPart(UriPartial.Path));
        writer.WriteNumber("iat", DateTimeOffset.UtcNow.ToUnixTimeSeconds());

        if (token is { Length: > 0 })
        {
            writer.WriteString("nonce", token);
        }

        using var start  = _encode.Get(buffer.WrittenMemory);
        var       result = _formatter.Get(start.AsMemory());
        return result;
    }
}

sealed class Hash : HashDataBase
{
    public static Hash Default { get; } = new();

    Hash() : base(SHA256.Create, Encoding.ASCII) {}
}

sealed class SignedDigest : IStopAware<string, ReadOnlyMemory<byte>>
{
    readonly IArray<string, byte> _hash;
    readonly IDeviceSigner        _signer;

    public SignedDigest(IDeviceSigner signer) : this(Hash.Default, signer) {}

    public SignedDigest(IArray<string, byte> hash, IDeviceSigner signer)
    {
        _hash   = hash;
        _signer = signer;
    }

    public async ValueTask<ReadOnlyMemory<byte>> Get(Stop<string> parameter)
    {
        var digest = _hash.Get(parameter).Open();
        var result = await _signer.Off(new(digest, parameter));
        return result;
    }
}

sealed class Signed : ISelect<ReadOnlyMemory<byte>, Leasing<char>>
{
    public static Signed Default { get; } = new();

    Signed() : this(DerToJose.Default, Base64UrlEncoder.Default) {}

    readonly ILease<ReadOnlyMemory<byte>, byte> _jose;
    readonly ILease<ReadOnlyMemory<byte>, char> _encode;

    public Signed(ILease<ReadOnlyMemory<byte>, byte> jose, ILease<ReadOnlyMemory<byte>, char> encode)
    {
        _jose        = jose;
        _encode = encode;
    }

    public Leasing<char> Get(ReadOnlyMemory<byte> parameter)
    {
        using var first = _jose.Get(parameter);
        return _encode.Get(first.AsMemory());
    }
}

sealed class Signature : IStopAware<string, Leasing<char>>
{
    readonly SignedDigest                                 _digest;
    readonly ISelect<ReadOnlyMemory<byte>, Leasing<char>> _signed;
    readonly ILease<ReadOnlyMemory<char>, char>           _formatter;
    
    public Signature(SignedDigest digest) : this(digest, Signed.Default, MemoryTokenFormatter.Default) {}

    public Signature(SignedDigest digest, ISelect<ReadOnlyMemory<byte>, Leasing<char>> signed,
                     ILease<ReadOnlyMemory<char>, char> formatter)
    {
        _digest    = digest;
        _signed    = signed;
        _formatter = formatter;
    }

    public async ValueTask<Leasing<char>> Get(Stop<string> parameter)
    {
        var       digest = await _digest.Off(parameter);
        using var signed = _signed.Get(digest);
        var       result = _formatter.Get(signed.AsMemory());
        return result;
    }
}

sealed class DerToJose : ILease<ReadOnlyMemory<byte>, byte>
{
    public static DerToJose Default { get; } = new();

    DerToJose() : this(32, NewLeasing<byte>.Default) {}

    readonly int               _part;
    readonly INewLeasing<byte> _new;

    public DerToJose(int part, INewLeasing<byte> @new)
    {
        _part     = part;
        _new = @new;
    }

    public Leasing<byte> Get(ReadOnlyMemory<byte> parameter)
    {
        int offset = 0;

        if (ReadByte() != 0x30) throw new CryptographicException("Invalid DER seq");
        _ = ReadLength();

        if (ReadByte() != 0x02) throw new CryptographicException("Invalid DER int r");
        var r = ReadInt();

        if (ReadByte() != 0x02) throw new CryptographicException("Invalid DER int s");
        var s = ReadInt();

        using var rPadded = LeftPad(r, _part);
        using var sPadded = LeftPad(s, _part);

        var result = _new.Get(_part * 2);
        var to     = result.AsSpan();
        rPadded.Memory.Span.CopyTo(to[.._part]);
        sPadded.Memory.Span.CopyTo(to.Slice(_part, _part));
        return result;

        byte ReadByte() => parameter.Span[offset++];

        ReadOnlySpan<byte> ReadSpan(int len)
        {
            var slice = parameter.Span.Slice(offset, len);
            offset += len;
            return slice;
        }

        int ReadLength()
        {
            int b = ReadByte();
            if (b >= 0x80)
            {
                var lenBytes = b & 0x7F;
                var len      = 0;
                for (var i = 0; i < lenBytes; i++)
                {
                    len = (len << 8) | ReadByte();
                }

                return len;
            }

            return b;
        }

        ReadOnlySpan<byte> ReadInt()
        {
            int len   = ReadByte();
            var slice = ReadSpan(len);
            return slice.Length > 0 && slice[0] == 0x00 ? slice[1..] : slice;
        }

        Lease<byte> LeftPad(ReadOnlySpan<byte> v, int size)
        {
            if (v.Length <= size)
            {
                var lease = _new.Get(size).AsEnumerable();
                var span  = lease.Memory.Span;

                v.CopyTo(span[(size - v.Length)..]);

                return lease;
            }

            throw new CryptographicException("Part too long");
        }

    }
}

sealed class CreateProof : IStopAware<CreateProofInput, string>
{
    readonly SigningInput      _input;
    readonly Signature         _signature;
    readonly INewLeasing<char> _leasing;

    public CreateProof(SigningInput input, Signature signature) : this(input, signature, NewLeasing<char>.Default) {}

    public CreateProof(SigningInput input, Signature signature, INewLeasing<char> leasing)
    {
        _input     = input;
        _signature = signature;
        _leasing   = leasing;
    }

    public async ValueTask<string> Get(Stop<CreateProofInput> parameter)
    {
        var (_, stop) = parameter;

        var       input     = await _input.Off(parameter);
        using var signature = await _signature.Off(new(input, stop));
        using var buffer    = _leasing.Get((uint)(input.Length + 1 + signature.Length));
        var       span      = buffer.AsSpan();
        input.AsSpan().CopyTo(span);
        span[input.Length] = '.';
        signature.AsSpan().CopyTo(span[(input.Length + 1)..]);
        return new(span);
    }
}

// --------------- Server nonce cache (per-origin) for retries ---------------
public interface ITokens : ISelect<Uri, string?>, ICommand<Pair<Uri, string>>;

public sealed class InMemoryTokens : ITokens
{
    readonly IDictionary<string, string> _map;

    public InMemoryTokens() : this(new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)) {}

    public InMemoryTokens(IDictionary<string, string> map) => _map = map;

    public string? Get(Uri origin) => _map.TryGetValue(origin.GetLeftPart(UriPartial.Authority), out var n) ? n : null;

    public void Execute(Pair<Uri, string> parameter)
    {
        var (origin, token)                            = parameter;
        _map[origin.GetLeftPart(UriPartial.Authority)] = token;
    }
}

sealed class CloneMessage : IAltering<HttpRequestMessage>
{
    public static CloneMessage Default { get; } = new();

    CloneMessage() {}

    public async ValueTask<HttpRequestMessage> Get(Stop<HttpRequestMessage> parameter)
    {
        var (message, stop) = parameter;

        var result = new HttpRequestMessage(message.Method, message.RequestUri);

        foreach (var h in message.Headers)
        {
            if (!string.Equals(h.Key, "DPoP", StringComparison.OrdinalIgnoreCase))
            {
                result.Headers.TryAddWithoutValidation(h.Key, h.Value);
            }
        }

        if (message.Content is not null)
        {
            var ms = new MemoryStream();
            await message.Content.CopyToAsync(ms, stop).Off();
            ms.Position    = 0;
            result.Content = new StreamContent(ms);
            foreach (var h in message.Content.Headers)
            {
                result.Content.Headers.TryAddWithoutValidation(h.Key, h.Value);
            }
        }

        return result;
    }
}

sealed class DevicePoPHandler : DelegatingHandler
{
    readonly IDeviceKeyProvider _keys;
    readonly CreateProof        _proof;
    readonly ITokens            _tokens;

    public DevicePoPHandler(IDeviceKeyProvider keys, CreateProof proof, ITokens tokens)
    {
        _keys   = keys;
        _proof  = proof;
        _tokens = tokens;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
    {
        // Authorization: DevicePoP <deviceId=jkt>
        var deviceId = (await _keys.Off(ct)).Jkt;
        request.Headers.Authorization = new AuthenticationHeaderValue("DevicePoP", deviceId);

        // DPoP proof (with last known nonce if any)
        var origin = new Uri(request.RequestUri!.GetLeftPart(UriPartial.Authority));
        var nonce  = _tokens.Get(origin);
        var proof  = await _proof.Off(new(new(request, nonce), ct));

        request.Headers.Remove("DPoP");
        request.Headers.TryAddWithoutValidation("DPoP", proof);

        // Send & handle 401 nonce challenge (retry once)
        var resp = await base.SendAsync(request, ct).Off();
        if ((int)resp.StatusCode == 401 && resp.Headers.TryGetValues("DPoP-Nonce", out var vals))
        {
            var newNonce = vals.FirstOrDefault();
            if (!string.IsNullOrEmpty(newNonce))
            {
                _tokens.Execute((origin, newNonce));

                resp.Dispose();

                var clone  = await CloneMessage.Default.Off(new(request, ct));
                var proof2 = await _proof.Off(new(new(clone, newNonce), ct));

                clone.Headers.Remove("DPoP");
                clone.Headers.TryAddWithoutValidation("DPoP", proof2);

                return await base.SendAsync(clone, ct).Off();
            }
        }

        // Cache next nonce if server supplies it on success
        if (resp.Headers.TryGetValues("DPoP-Nonce", out var next))
        {
            var n = next.FirstOrDefault();
            if (!n.IsNullOrEmpty())
            {
                _tokens.Execute((origin, n));
            }
        }

        return resp;
    }
}

// --------------- Passkey Flow (client side) ---------------
/*
public sealed class PasskeyClient
{
    readonly HttpClient _deviceApi; // configured with DevicePoPHandler

    public PasskeyClient(HttpClient deviceApi)
    {
        _deviceApi = deviceApi;
    }

    // 1) Begin-passkey: request a ticket JWT + url to open system browser
    public async Task<(string authorizeUrl, string ticketJwt)> BeginPasskeyAsync(CancellationToken ct)
    {
        var req  = new HttpRequestMessage(HttpMethod.Post, "/api/begin-passkey");
        var resp = await _deviceApi.SendAsync(req, ct);
        resp.EnsureSuccessStatusCode();

        using var s      = await resp.Content.ReadAsStreamAsync(ct);
        using var doc    = await JsonDocument.ParseAsync(s, cancellationToken: ct);
        var       url    = doc.RootElement.GetProperty("authorizeUrl").GetString()!;
        var       ticket = doc.RootElement.GetProperty("ticket").GetString()!;
        return (url, ticket);
    }

    // 2) Open system browser with the URL (including ticket as query param)
    public Task OpenSystemBrowserAsync(string authorizeUrl)
    {
        // MAUI: use Launcher.OpenAsync
        return Microsoft.Maui.ApplicationModel.Launcher.OpenAsync(new Uri(authorizeUrl));
    }

    // 3) Handle deep link back to app: you’ll get a JWE from your redirect URI
    //    Then finish passkey by POSTing that JWE to the server (DevicePoP protected).
    public async Task<(string accessToken, string refreshToken)> FinishPasskeyAsync(
        string jweFromDeepLink, CancellationToken ct)
    {
        var payload = JsonSerializer.Serialize(new { jwe = jweFromDeepLink });
        var req = new HttpRequestMessage(HttpMethod.Post, "/api/finish-passkey")
        {
            Content = new StringContent(payload, Encoding.UTF8, "application/json")
        };
        var resp = await _deviceApi.SendAsync(req, ct);
        resp.EnsureSuccessStatusCode();

        using var s   = await resp.Content.ReadAsStreamAsync(ct);
        using var doc = await JsonDocument.ParseAsync(s, cancellationToken: ct);
        var       at  = doc.RootElement.GetProperty("access_token").GetString()!;
        var       rt  = doc.RootElement.GetProperty("refresh_token").GetString()!;
        return (at, rt);
    }
}
*/

// --------------- DI wiring (MauiProgram.cs) ---------------
/*
builder.Services.AddSingleton<IServerNonceCache, InMemoryNonceCache>();

// TODO: Plug your real platform implementations using Secure Enclave / Keystore:
builder.Services.AddTransient<IDeviceKeyProvider, DeviceKeyProvider>();
builder.Services.AddTransient<IDeviceSigner, DeviceSigner>();

builder.Services.AddHttpClient("DeviceApi", client =>
{
    client.BaseAddress = new Uri("https://api.yourbank.com"); // adjust
})
.AddHttpMessageHandler(sp =>
{
    var keys = sp.GetRequiredService<IDeviceKeyProvider>();
    var signer = sp.GetRequiredService<IDeviceSigner>();
    var nonces = sp.GetRequiredService<IServerNonceCache>();
    return new DevicePoPHandler(new HttpClientHandler(), keys, signer, nonces);
});

builder.Services.AddTransient(sp =>
    new PasskeyClient(sp.GetRequiredService<IHttpClientFactory>().CreateClient("DeviceApi")));
*/

// --------------- Platform placeholders (implement with your attested key) ---------------
public sealed class DeviceKeyProvider : IDeviceKeyProvider
{
    public ValueTask<PublicJWK> Get(CancellationToken parameter)
    {
        // iOS: extract x/y from SecKey public; Android: from ECPublicKey ECPoint
        // Compute JKT = RFC7638 thumbprint of {"kty":"EC","crv":"P-256","x":"...","y":"..."}
        throw new NotImplementedException("Implement using your hardware key.");
    }
}

public sealed class DeviceSigner : IDeviceSigner
{
    public ValueTask<ReadOnlyMemory<byte>> Get(Stop<ReadOnlyMemory<byte>> parameter)
    {
        // iOS: SecKey.CreateSignature(SecKeyAlgorithm.EcdsaSignatureDigestX962Sha256)
        // Android: Signature.getInstance("NONEwithECDSA") with AndroidKeyStore private key
        throw new NotImplementedException("Implement using your hardware private key.");
    }
}