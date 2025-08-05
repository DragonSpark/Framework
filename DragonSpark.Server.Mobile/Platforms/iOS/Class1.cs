using System;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Results.Stop;
using DragonSpark.Model.Sequences;
using DragonSpark.Model.Sequences.Memory;
using Microsoft.Extensions.DependencyInjection;
using PeterO.Cbor;

namespace DragonSpark.Server.Mobile.Platforms.iOS;

public sealed class Registrations : ICommand<IServiceCollection>
{
    public void Execute(IServiceCollection parameter) {}
}

sealed class AppleCertificate : Text.Text
{
    public static AppleCertificate Default { get; } = new();

    AppleCertificate() : base("https://www.apple.com/certificateauthority/Apple_App_Attestation_Root_CA.pem") {}
}

sealed class RootCertificate : IStopAware<Array<byte>>
{
    readonly IHttpClientFactory _clients;
    readonly Uri                _address;

    public RootCertificate(IHttpClientFactory clients) : this(clients, new(AppleCertificate.Default)) {}

    public RootCertificate(IHttpClientFactory clients, Uri address)
    {
        _clients = clients;
        _address = address;
    }

    public async ValueTask<Array<byte>> Get(CancellationToken parameter)
    {
        using var client   = _clients.CreateClient();
        var       response = await client.GetAsync(_address, parameter).Off();
        var       result   = await response.Content.ReadAsByteArrayAsync(parameter).Off();
        return result;
    }
}

public readonly record struct VerificationInput(string Challenge, string Input);

sealed class Verify : Model.Operations.Selection.Stop.IDepending<VerificationInput>
{
    readonly IStopAware<Array<byte>> _root;

    public Verify(RootCertificate root) => _root = root;

    public async ValueTask<bool> Get(Stop<VerificationInput> parameter)
    {
        var ((challenge, input), stop) = parameter;
        // Detailed verification steps (use libraries: PeterO.Cbor for CBOR, BouncyCastle for crypto)
        var actual = Convert.FromBase64String(input);

        // Step 1: Decode CBOR
        var decode = CBORObject.DecodeFromBytes(actual);
        var body   = decode["attStmt"];
        var pair   = body["x5c"].Values.Select(x => x.GetByteString()).ToArray();

        // Step 2: Verify certificate chain
        var certificate  = X509CertificateLoader.LoadCertificate(pair[0]);
        var intermediate = X509CertificateLoader.LoadCertificate(pair[1]);
        var root         = X509CertificateLoader.LoadCertificate(await _root.Off(stop));
        var chain        = new X509Chain();
        var store        = chain.ChainPolicy.ExtraStore;
        store.Add(intermediate);
        store.Add(root);

        var valid = chain.Build(certificate);
        if (valid)
        {
            var       data    = decode["authData"].GetByteString();
            var       hash    = SHA256.HashData(Convert.FromBase64String(challenge));
            using var leasing = NewLeasing<byte>.Default.Get(data.Length + hash.Length);
            var       nonce   = leasing.Store;
            Buffer.BlockCopy(data, 0, nonce, 0, data.Length);
            Buffer.BlockCopy(hash, 0, nonce, data.Length, hash.Length);
            var expected = SHA256.HashData(leasing.AsSpan());

            var receipt = body["receipt"].GetByteString();
            // Additional checks: app ID, environment, etc.
            // Store receipt/public key hash for future assertions

            return expected.SequenceEqual(certificate.GetCertHash(HashAlgorithmName.SHA256));
        }

        return false;
    }
}