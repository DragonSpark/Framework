using DragonSpark.Model.Commands;

namespace DragonSpark.Application.AspNet.Entities.Editing;

public sealed class AddLocal<T> : Command<Edit<T>>, IModify<T> where T : class
{
	public static AddLocal<T> Default { get; } = new();

	AddLocal() : base(x => x.Add(x.Subject)) {}
}