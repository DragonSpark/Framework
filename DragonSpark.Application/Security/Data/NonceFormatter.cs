using DragonSpark.Text;

namespace DragonSpark.Application.Security.Data;

public sealed class NonceFormatter : IFormatter<string>
{
    public static NonceFormatter Default { get; } = new();

    NonceFormatter() {}

    public string Get(string parameter) => parameter.Replace('+', '-').Replace('/', '_').TrimEnd('=');
}