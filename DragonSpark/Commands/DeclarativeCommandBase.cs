using PostSharp.Patterns.Contracts;

namespace DragonSpark.Commands
{
	public abstract class DeclarativeCommandBase<T> : CommandBase<object>
	{
		[Required]
		public T Parameter { [return: Required]get; set; }
	}
}