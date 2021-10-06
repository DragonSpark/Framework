using DragonSpark.Compose;

namespace DragonSpark.Application.Entities.Editing
{
	// TODO: Rename -> Update
	public class Save<T> : Modify<T> where T : class
	{
		protected Save(IScopes scopes) : base(scopes, UpdateLocal<T>.Default.Then().Operation()) {}
	}

	// TODO: Rename -> Save
	public sealed class StandardSave<T> : Save<T> where T : class
	{
		public StandardSave(IStandardScopes scopes) : base(scopes) {}
	}
}