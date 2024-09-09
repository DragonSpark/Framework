using DragonSpark.Compose.Model.Validation;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Conditions;
using System;
using System.Linq;

namespace DragonSpark.Compose.Model.Selection;

public class ConditionComposer<T> : Composer<T, bool>
{
	public static implicit operator Predicate<T>(ConditionComposer<T> instance) => instance.Get().Get;

	public static implicit operator Func<T, bool>(ConditionComposer<T> instance) => instance.Get().Get;

	public ConditionComposer(ISelect<T, bool> subject) : base(subject) {}

	public ConditionComposer<T> Or(params Func<T, bool>[] others)
		=> Or(others.Select(x => new Condition<T>(x)).Open<ISelect<T, bool>>());

	public ConditionComposer<T> Or(params ISelect<T, bool>[] others)
		=> new(new AnyCondition<T>(others.Prepend(Get()).Open()));

	public ConditionComposer<T> And(params Func<T, bool>[] others)
		=> And(others.Select(x => new Condition<T>(x)).Open<ISelect<T, bool>>());

	public ConditionComposer<T> And(params ISelect<T, bool>[] others)
		=> new(new AllCondition<T>(others.Prepend(Get()).Open()));

	public ElseComposer<T> Then(ICondition<T> subject) => Then(subject.Get);

	public ElseComposer<T> Then(Func<T, bool> subject) => new(Get().Get, subject);

	public ConditionComposer<T> Inverse() => new(InverseConditions<T>.Default.Get(new Condition<T>(Get().Get)));
}