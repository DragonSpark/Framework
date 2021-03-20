using DragonSpark.Model.Selection.Alterations;
using NetFabric.Hyperlinq;
using System.Security.Cryptography;
using System.Text;

namespace DragonSpark.Application.Security
{
	public sealed class IdentifierHash : IAlteration<string>
	{
		public static IdentifierHash Default { get; } = new IdentifierHash();

		IdentifierHash() : this(Encoding.ASCII) {}

		readonly Encoding _encoding;

		public IdentifierHash(Encoding encoding) => _encoding = encoding;

		public string Get(string parameter)
		{
			using var context = MD5.Create();
			var       hash    = context.ComputeHash(_encoding.GetBytes(parameter));
			var       parts   = hash.AsValueEnumerable().Select(x => x.ToString("x2")).ToArray();
			var       result  = string.Join(string.Empty, parts);
			return result;
		}
	}
}