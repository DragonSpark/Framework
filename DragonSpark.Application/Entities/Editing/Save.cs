using DragonSpark.Compose;

namespace DragonSpark.Application.Entities.Editing
{
	public class Save<T> : Modify<T> where T : class
	{
		protected Save(IScopes scopes) : base(scopes, UpdateLocal<T>.Default.Then().Operation()) {}
	}

	public sealed class StandardSave<T> : Save<T> where T : class
	{
		public StandardSave(IStandardScopes scopes) : base(scopes) {}
	}
}