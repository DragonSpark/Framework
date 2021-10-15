using DragonSpark.Model.Commands;

namespace DragonSpark.Application.Entities.Editing;

public sealed class AttachLocal<T> : Command<Edit<T>>, IModify<T> where T : class
{
	public static AttachLocal<T> Default { get; } = new AttachLocal<T>();

	AttachLocal() : base(x => x.Attach(x.Subject)) {}
}