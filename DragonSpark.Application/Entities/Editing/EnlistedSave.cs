namespace DragonSpark.Application.Entities.Editing
{
	public class EnlistedSave<T> : Update<T> where T : class
	{
		public EnlistedSave(IEnlistedScopes scopes) : base(scopes) {}
	}
}