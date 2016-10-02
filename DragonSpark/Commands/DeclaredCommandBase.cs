using DragonSpark.Extensions;
using PostSharp.Patterns.Contracts;

namespace DragonSpark.Commands
{
	public abstract class DeclaredCommandBase<T> : CommandBase<object>
	{
		protected DeclaredCommandBase( T parameter = default(T) )
		{
			if ( parameter.IsAssigned() )
			{
				Parameter = parameter;
			}
		}

		[Required]
		public T Parameter { [return: Required]get; set; }
	}
}