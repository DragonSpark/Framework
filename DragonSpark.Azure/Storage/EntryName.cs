using DragonSpark.Model.Selection;
using DragonSpark.Text;

namespace DragonSpark.Azure.Storage;

sealed class EntryName : IFormatter<EntryInput>
{
	public static EntryName Default { get; } = new();

	EntryName() : this(FileNameProperty.Default) {}

	readonly ISelect<IDictionary<string, string?>, string?> _name;

	public EntryName(ISelect<IDictionary<string, string?>, string?> name) => _name = name;

	public string Get(EntryInput parameter)
	{
		var (client, properties) = parameter;
		if (properties.Metadata.Count > 0)
		{
			var located = _name.Get(properties.Metadata);
			if (located is not null)
			{
				return located;
			}
		}

		return System.IO.Path.GetFileName(client.Name);
	}
}