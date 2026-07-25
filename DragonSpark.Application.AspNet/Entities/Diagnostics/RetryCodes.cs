using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Application.AspNet.Entities.Diagnostics;

sealed class RetryCodes : Condition<int>
{
	public static RetryCodes Default { get; } = new();

	RetryCodes() : base(new[] { -2, 121, 10060 }.Contains) {}
}