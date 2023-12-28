using DragonSpark.Model.Sequences;

namespace DragonSpark.Identity.PayPal;

sealed class KnownClaims : Instances<string>
{
	public static KnownClaims Default { get; } = new();

	KnownClaims() : base(PayIdentifier.Default, Country.Default) {}
}