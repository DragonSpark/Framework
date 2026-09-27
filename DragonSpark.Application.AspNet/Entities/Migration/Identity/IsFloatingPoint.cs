using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Application.AspNet.Entities.Migration.Identity;

sealed class IsFloatingPoint : Condition<object>
{
	public static IsFloatingPoint Default { get; } = new();

	IsFloatingPoint()
		: base(x => x is IConvertible c && c.GetTypeCode() is TypeCode.Single or TypeCode.Double or TypeCode.Decimal) {}
}