using System.Security.Cryptography;
using System.Text;

namespace DragonSpark.Application.Security;

public sealed class HashData : HashDataBase
{
    public HashData(Func<HashAlgorithm> hash, Encoding encoding) : base(hash, encoding) {}
}