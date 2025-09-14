using DragonSpark.Application.Security;
using DragonSpark.Compose;
using DragonSpark.Model.Results;

namespace DragonSpark.Application.AspNet.Security;

sealed class ContentPolicyNonce : Result<string>
{
	public static ContentPolicyNonce Default { get; } = new();

	ContentPolicyNonce() : base(HexNonce.Default.Then().Bind(16)) {}
}