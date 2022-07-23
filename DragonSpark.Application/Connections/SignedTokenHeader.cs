using DragonSpark.Application.Communication;

namespace DragonSpark.Application.Connections;

sealed class SignedTokenHeader : Header
{
	public static SignedTokenHeader Default { get; } = new();

	SignedTokenHeader() : base(SignedTokenHeaderName.Default) {}
}