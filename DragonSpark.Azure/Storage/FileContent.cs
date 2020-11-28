using DragonSpark.Model.Sequences;
using JetBrains.Annotations;

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

		public void Deconstruct([NotNull] out string name, [NotNull] out string contentType, out Array<byte> content)
		{
			name        = Name;
			contentType = ContentType;
			content     = Content;
		}
	}
}