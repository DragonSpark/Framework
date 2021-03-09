using DragonSpark.Model.Selection.Alterations;
using NetFabric.Hyperlinq;
using System.Buffers;
using System.Security.Cryptography;
using System.Text;

namespace DragonSpark.Application.Security
{
	public sealed class Hash : IAlteration<string>
	{
		public static Hash Default { get; } = new Hash();

		Hash() : this(Encoding.ASCII) {}

		readonly Encoding _encoding;

		public Hash(Encoding encoding) => _encoding = encoding;

		public string Get(string parameter)
		{
			using var context = MD5.Create();
			var       hash = context.ComputeHash(_encoding.GetBytes(parameter));
			using var parts = hash.AsValueEnumerable().Select(x => x.ToString("x2")).ToArray(MemoryPool<string>.Shared);
			var       result = string.Join(string.Empty, parts);
			return result;
		}
	}
}