using DragonSpark.Compose;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Model.Sequences;
using System;
using System.Security.Cryptography;
using System.Text;

namespace DragonSpark.Server.Security;

public class Hasher : IAlteration<string>
{
	readonly Encoding   _encoding;
	readonly Func<HMAC> _hasher;

	protected Hasher(Func<Array<byte>, HMAC> hasher, string key) : this(hasher, key, Encoding.ASCII) {}

	protected Hasher(Func<Array<byte>, HMAC> hasher, string key, Encoding encoding)
		: this(hasher.Start().Bind(encoding.GetBytes(key)), encoding) {}

	protected Hasher(Func<HMAC> hasher, Encoding encoding)
	{
		_hasher   = hasher;
		_encoding = encoding;
	}

	public string Get(string parameter)
	{
		using var hmac = _hasher();
		var result = BitConverter.ToString(hmac.ComputeHash(_encoding.GetBytes(parameter))).Replace("-", string.Empty);
		return result;
	}
}