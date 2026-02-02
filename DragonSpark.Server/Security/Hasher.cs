using System;
using System.Security.Cryptography;
using DragonSpark.Compose;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Model.Sequences;
using DragonSpark.Text;

namespace DragonSpark.Server.Security;

public class Hasher : IAlteration<string>
{
	readonly Func<HMAC>      _hasher;
    readonly IParser<byte[]> _parser;

    protected Hasher(Func<Array<byte>, HMAC> hasher, string key)
        : this(hasher, EncodedTextAsData.Default, key) {}

    protected Hasher(Func<Array<byte>, HMAC> hasher, IParser<byte[]> parser, string key)
        : this(hasher, parser, parser.Get(key)) {}

    protected Hasher(Func<Array<byte>, HMAC> hasher, IParser<byte[]> parser, Array<byte> key)
        : this(hasher.Start().Bind(key), parser) {}

	protected Hasher(Func<HMAC> hasher, IParser<byte[]> parser)
	{
		_hasher      = hasher;
        _parser = parser;
	}

	public string Get(string parameter)
	{
		using var hmac = _hasher();
		var result = BitConverter.ToString(hmac.ComputeHash(_parser.Get(parameter))).Replace("-", string.Empty);
		return result;
	}
}