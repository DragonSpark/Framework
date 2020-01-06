using DragonSpark.Compose;
using DragonSpark.Model.Selection.Conditions;
using System;
using System.Reflection;

namespace DragonSpark.Reflection
{
	sealed class IsDefined<T> : Condition<ICustomAttributeProvider>
	{
		public static IsDefined<T> Default { get; } = new IsDefined<T>();

		public static IsDefined<T> Inherited { get; } = new IsDefined<T>(true);

		IsDefined() : this(false) {}

		public IsDefined(bool inherit) : this(A.Type<T>(), inherit) {}

		public IsDefined(Type type, bool inherit) : base(new IsDefined(type, inherit).Get) {}

		public bool IsSatisfiedBy(ICustomAttributeProvider parameter) => Get(parameter);
	}

	sealed class IsDefined : ICondition<ICustomAttributeProvider>
	{
		readonly bool _inherit;

		readonly Type _type;

		public IsDefined(Type type, bool inherit)
		{
			_type    = type;
			_inherit = inherit;
		}

		public bool Get(ICustomAttributeProvider parameter) => parameter.IsDefined(_type, _inherit);
	}
}