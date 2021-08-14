namespace DragonSpark.Application.Security.Identity.Claims.Access
{
	public readonly struct Accessed
	{
		public Accessed(string claim, bool exists, string? value)
		{
			Claim  = claim;
			Exists = exists;
			Value  = value;
		}

		public string Claim { get; }

		public bool Exists { get; }

		public string? Value { get; }

		public void Deconstruct(out string claim, out bool exists, out string? value)
		{
			claim  = Claim;
			exists = Exists;
			value  = Value;
		}
	}
}