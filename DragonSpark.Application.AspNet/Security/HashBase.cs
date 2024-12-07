using DragonSpark.Model.Selection.Alterations;
using NetFabric.Hyperlinq;
using System;
using System.Security.Cryptography;
using System.Text;

namespace DragonSpark.Application.Security;

public class HashBase : IAlteration<string>
{
	readonly Func<HashAlgorithm> _hash;
	readonly Encoding            _encoding;

	public HashBase(Func<HashAlgorithm> hash, Encoding encoding)
	{
		_hash     = hash;
		_encoding = encoding;
	}

	public string Get(string parameter)
	{
		using var context = _hash();
		var       hash    = context.ComputeHash(_encoding.GetBytes(parameter));
		var       parts   = hash.AsValueEnumerable().Select(x => x.ToString("x2")).ToArray();
		var       result  = string.Join(string.Empty, parts);
		return result;
	}
}