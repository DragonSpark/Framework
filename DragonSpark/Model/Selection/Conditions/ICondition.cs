namespace DragonSpark.Model.Selection.Conditions
{
	public interface ICondition : ICondition<None> {}

	public interface ICondition<in T> : ISelect<T, bool> {}
}