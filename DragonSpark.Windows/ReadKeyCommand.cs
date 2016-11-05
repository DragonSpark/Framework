using System;

namespace DragonSpark.Windows
{
	public sealed class InputOutput : Application.Setup.InputOutputBase
	{
		public static InputOutput Default { get; } = new InputOutput();
		InputOutput() : base( Console.Out, Console.In ) {}
	}
}