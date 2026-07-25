namespace DragonSpark.Model.Selection.Conditions;

public class Condition<T> : Select<T, bool>, ICondition<T>
{
	public Condition(ISelect<T, bool> select) : base(select) {}

	public Condition(Func<T, bool> select) : base(select) {}
}

public class Condition : Select<None, bool>, ICondition
{
	public Condition(ISelect<None, bool> select) : base(select) {}

	public Condition(Func<None, bool> select) : base(select) {}
}