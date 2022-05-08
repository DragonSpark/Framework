namespace DragonSpark.Application.Entities.Editing;

public class MarkManyForRemoval<T> : ModifyMany<T> where T : class
{
	protected MarkManyForRemoval(SessionEditors editors) : base(editors, RemoveLocal<T>.Default) {}
}