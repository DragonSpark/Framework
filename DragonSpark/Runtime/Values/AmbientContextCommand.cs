using System.Collections.Generic;
using System.Linq;
using PostSharp.Patterns.Contracts;

namespace DragonSpark.Runtime.Values
{
	public class AmbientContextCommand<T> : StackCommand<T>
	{
		readonly ThreadAmbientValue<T> value;

		public AmbientContextCommand() : this( new ThreadAmbientValue<T>() ) {}

		public AmbientContextCommand( [Required]ThreadAmbientValue<T> value ) : base( value.Item )
		{
			this.value = value;
		}

		protected override void OnDispose()
		{
			base.OnDispose();
			if ( !value.Item.Any() )
			{
				value.Dispose();
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