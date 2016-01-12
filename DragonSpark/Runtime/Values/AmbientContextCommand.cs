using System.Collections.Generic;
using System.Linq;
using PostSharp.Patterns.Contracts;

namespace DragonSpark.Runtime.Values
{
	public class AmbientContextCommand<T> : StackCommand<T>
	{
		readonly ThreadAmbientChain<T> chain;

		public AmbientContextCommand() : this( new ThreadAmbientChain<T>() ) {}

		public AmbientContextCommand( [Required]ThreadAmbientChain<T> chain ) : base( chain.Item )
		{
			this.chain = chain;
		}

		protected override void OnDispose()
		{
			base.OnDispose();
			if ( !chain.Item.Any() )
			{
				chain.Dispose();
			}
		}
	}

	public class StackCommand<T> : DisposingCommand<T>
	{
		readonly Stack<T> stack;

		public StackCommand( [Required]Stack<T> stack )
		{
			this.stack = stack;
		}

		protected override void OnExecute( T parameter ) => stack.Push( parameter );

		protected override void OnDispose() => stack.Pop();
	}
}