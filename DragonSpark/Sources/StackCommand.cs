using DragonSpark.Commands;

namespace DragonSpark.Sources
{
	public class StackCommand<T> : DisposingCommandBase<T>
	{
		public StackCommand( IStack<T> stack )
		{
			Stack = stack;
		}

		protected IStack<T> Stack { get; }

		public override void Execute( T parameter ) => Stack.Push( parameter );

		protected override void OnDispose() => Stack.Pop();
	}
}