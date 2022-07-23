using DragonSpark.Compose;
using DragonSpark.Model.Results;
using System.Runtime.InteropServices;

namespace DragonSpark.Application.Connections;

[Guid("25B20153-2ABF-4513-A3E5-9A3CCD96D019")]
sealed class ExpectedTokenContent : Instance<string>
{
	public static ExpectedTokenContent Default { get; } = new();

	ExpectedTokenContent() : base(A.Type<ExpectedTokenContent>().GUID.ToString()) {}
}