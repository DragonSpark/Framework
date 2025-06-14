using DragonSpark.Text;

namespace DragonSpark.Azure.Storage;

sealed class EntryName : IFormatter<EntryInput>
{
	public static EntryName Default { get; } = new();

	EntryName() {}

	public string Get(EntryInput parameter)
	{
		var (client, properties) = parameter;
		if (properties.Metadata.Count > 0)
		{
			var located = new FileNameVariable(properties.Metadata).Get();
			if (located is not null)
			{
				return located;
			}
		}

		return System.IO.Path.GetFileName(client.Name);
	}
}