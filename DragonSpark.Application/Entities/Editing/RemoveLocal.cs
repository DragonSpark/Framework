using DragonSpark.Model.Commands;

namespace DragonSpark.Application.Entities.Editing;

public sealed class RemoveLocal<T> : Command<Edit<T>>, IModify<T> where T : class
{
	public static RemoveLocal<T> Default { get; } = new RemoveLocal<T>();

	RemoveLocal() : base(x => x.Remove(x.Subject)) {}
}