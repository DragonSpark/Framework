using System;

namespace DragonSpark.Windows.Setup
{
	public class InputOutput : Application.Setup.InputOutput
	{
		public static InputOutput Default { get; } = new InputOutput();
		InputOutput() : base( Console.Out, Console.In ) {}
	}
}