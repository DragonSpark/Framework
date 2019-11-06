using DragonSpark.Model.Selection;

namespace DragonSpark.Text
{
	public interface IFormatter<in T> : ISelect<T, string> {}

	/*public interface IUtf8 : ISelect<Utf8Input, uint> {}

	public readonly struct Utf8Input
	{
		public Utf8Input(ReadOnlyMemory<char> characters, byte[] destination, in uint start)
		{
			Characters  = characters;
			Destination = destination;
			Start       = start;
		}

		public ReadOnlyMemory<char> Characters { get; }

		public byte[] Destination { get; }

		public uint Start { get; }
	}*/
}