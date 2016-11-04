using System.IO;

namespace DragonSpark.Application.Setup
{
	public interface IInputOutput
	{
		TextReader Reader { get; }
		TextWriter Writer { get; }
	}

	public abstract class InputOutputBase : IInputOutput
	{
		protected InputOutputBase( TextWriter writer, TextReader reader )
		{
			Writer = writer;
			Reader = reader;
		}

		public TextWriter Writer { get; }
		public TextReader Reader { get; }
	}
}