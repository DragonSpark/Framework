using DragonSpark.Compose;
using DragonSpark.Model.Selection.Conditions;
using System;

namespace DragonSpark.Model;

sealed class AssignedResultGuard<T> : AssignedGuard<T, InvalidOperationException>
{
	public static AssignedResultGuard<T> Default { get; } = new AssignedResultGuard<T>();

	AssignedResultGuard() : base(AssignedResultMessage.Default.Then().Out<T>()) {}

	public AssignedResultGuard(ICondition<T> condition, Func<T, string> message) : base(condition, message) {}
}