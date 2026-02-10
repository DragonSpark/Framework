using DragonSpark.Model.Results;
using Java.Security;

namespace DragonSpark.Application.Mobile.Maui.Platforms.Android.Security.Tokens;

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
        => parameter.ContainsAlias(_alias) ? _previous.Get(parameter) : _generate.Get();
}