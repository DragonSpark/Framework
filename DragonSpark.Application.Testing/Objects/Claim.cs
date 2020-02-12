namespace DragonSpark.Application.Testing.Objects {
	class Claim : Text.Text
	{
		protected Claim(string name) : base($"{ClaimNamespace.Default}:{name}") {}
	}
}