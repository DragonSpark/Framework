namespace DragonSpark.Model.Selection.Conditions
{
	sealed class Never : Condition<object>
	{
		public static Never Default { get; } = new Never();

		Never() : base(Never<object>.Default.Get) {}
	}

	sealed class Never<T> : FixedResultCondition<T>
	{
		public static ICondition<T> Default { get; } = new Never<T>();

		Never() : base(false) {}
	}
}