using System;

namespace DragonSpark.Text;

public sealed class Base64EncodeData : Formatter<byte[]>
{
    public static Base64EncodeData Default { get; } = new();

    Base64EncodeData() : base(Convert.ToBase64String) {}
}