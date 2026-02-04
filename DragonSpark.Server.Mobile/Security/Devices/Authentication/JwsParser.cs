using System;
using System.Text;
using DragonSpark.Model.Sequences.Memory;
using DragonSpark.Server.Mobile.Security.Devices.Cryptography;
using DragonSpark.Text;

namespace DragonSpark.Server.Mobile.Security.Devices.Authentication;

sealed class JwsParser : IParser<JwsResult?>
{
    public static JwsParser Default { get; } = new();

    JwsParser() : this(Base64UrlDecode.Default, ComposeJwsParserInput.Default, ComposeSignature.Default) {}

    readonly ILease<ReadOnlyMemory<char>, byte> _decode;
    readonly IParser<JwsParserInput?>           _input;
    readonly ILease<ReadOnlyMemory<char>, byte> _signature;

    public JwsParser(ILease<ReadOnlyMemory<char>, byte> decode, IParser<JwsParserInput?> input,
                     ILease<ReadOnlyMemory<char>, byte> signature)
    {
        _decode    = decode;
        _input     = input;
        _signature = signature;
    }

    public JwsResult? Get(string parameter)
    {
        var input = _input.Get(parameter);
        if (input is not null)
        {
            var (first, next, second, all) = input.Value;
            try
            {
                var memory       = parameter.AsMemory();
                var signingInput = memory[..all]; // "<hdr>.<pl>"
                var rest         = memory[next..];
                var hdrJson      = _decode.Get(memory[..first]);
                var plJson       = _decode.Get(rest[..second]);
                var signature    = _signature.Get(rest[(second + 1)..]);
                return new(hdrJson, plJson, signingInput, signature);
            }
            catch (Exception e) when (e is DecoderFallbackException or FormatException or ArgumentException)
            {
                return null;
            }
        }

        return null;
    }
}