using DragonSpark.Compose;
using DragonSpark.Model.Selection.Conditions;
using System;

namespace DragonSpark.Model
{
	public class AssignedGuard<T, TException> : Guard<T, TException> where TException : Exception
	{
		public AssignedGuard(Func<T, string> message) : this(Is.Assigned<T>().Inverse().Out(), message) {}

		public AssignedGuard(ICondition<T> condition, Func<T, string> message) : base(condition, message) {}
	}
}