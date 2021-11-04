using DragonSpark.Model.Selection.Alterations;
using NetFabric.Hyperlinq;
using System.Security.Cryptography;
using System.Text;

namespace DragonSpark.Application.Security
{
	public sealed class Hash : IAlteration<string>
	{
		public static Hash Default { get; } = new();

		Hash() : this(Encoding.UTF8) {}

		readonly Encoding _encoding;

		public Hash(Encoding encoding) => _encoding = encoding;

		public string Get(string parameter)
		{
			using var context = SHA256.Create();
			var       hash    = context.ComputeHash(_encoding.GetBytes(parameter));
			var       parts   = hash.AsValueEnumerable().Select(x => x.ToString("x2")).ToArray();
			var       result  = string.Join(string.Empty, parts);
			return result;
		}
	}
}
