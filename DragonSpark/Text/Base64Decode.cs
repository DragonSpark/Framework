using System;
using System.Text;

namespace DragonSpark.Text;

public sealed class Base64Decode : Base64DecodeBase
{
    public static Base64Decode Default { get; } = new();

    Base64Decode() : this(Encoding.UTF8) {}

    public Base64Decode(Encoding encoding) : base(Convert.FromBase64String, encoding) {}
}