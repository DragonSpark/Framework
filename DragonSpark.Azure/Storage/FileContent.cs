using DragonSpark.Model.Sequences;

namespace DragonSpark.Azure.Storage
{
	public readonly struct FileContent
	{
		public FileContent(string name, string contentType, Array<byte> content)
		{
			Name        = name;
			ContentType = contentType;
			Content     = content;
		}

		public string Name { get; }

		public string ContentType { get; }

		public Array<byte> Content { get; }

		public void Deconstruct(out string name, out string contentType, out Array<byte> content)
		{
			name        = Name;
			contentType = ContentType;
			content     = Content;
		}
	}
}