using System.Security.Cryptography;
using System.Text;

namespace DragonSpark.Application.Security.Tokens;

sealed class Hash : HashDataBase
{
    public static Hash Default { get; } = new();

    Hash() : base(SHA256.Create, Encoding.ASCII) {}
}