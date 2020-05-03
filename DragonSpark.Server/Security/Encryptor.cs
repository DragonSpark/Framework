using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Model.Sequences;
using System;
using System.Security.Cryptography;
using System.Text;

namespace DragonSpark.Server.Security
{
	sealed class Encryptor : IAlteration<string>
	{
		readonly Encoding    _encoding;
		readonly Array<byte> _key;

		public Encryptor(string key) : this(key, Encoding.ASCII) {}

		public Encryptor(string key, Encoding encoding) : this(encoding.GetBytes(key), encoding) {}

		public Encryptor(Array<byte> key, Encoding encoding)
		{
			_key      = key;
			_encoding = encoding;
		}

		public string Get(string parameter)
		{
			var       key  = _key;
			using var hmac = new HMACSHA512(key);
			var result = BitConverter.ToString(hmac.ComputeHash(_encoding.GetBytes(parameter)))
			                         .Replace("-", string.Empty);
			return result;
		}
	}
}