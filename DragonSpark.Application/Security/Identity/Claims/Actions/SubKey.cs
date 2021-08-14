namespace DragonSpark.Application.Security.Identity.Claims.Actions
{
	public readonly struct SubKey
	{
		public SubKey(string key, string element)
		{
			Key     = key;
			Element = element;
		}

		public string Key { get; }

		public string Element { get; }

		public void Deconstruct(out string key, out string element)
		{
			key     = Key;
			element = Element;
		}
	}
}