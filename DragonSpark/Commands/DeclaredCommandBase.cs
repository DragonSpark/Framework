using PostSharp.Patterns.Contracts;

namespace DragonSpark.Commands
{
	public abstract class DeclaredCommandBase<T> : CommandBase<object>
	{
		[Required]
		public T Parameter { [return: Required]get; set; }
	}
}