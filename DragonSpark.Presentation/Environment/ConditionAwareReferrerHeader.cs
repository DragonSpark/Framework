using DragonSpark.Application.Communication;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Conditions;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Presentation.Environment;

public class ConditionAwareReferrerHeader : ISelect<HttpRequest, string?>
{
	readonly IHeader            _header;
	readonly ICondition<string> _condition;

	public ConditionAwareReferrerHeader(ICondition<string> condition) : this(ReferrerHeader.Default, condition) {}

	public ConditionAwareReferrerHeader(IHeader header, ICondition<string> condition)
	{
		_header    = header;
		_condition = condition;
	}

	public string? Get(HttpRequest parameter)
	{
		var referrer = _header.Get(parameter.Headers);
		var result   = !string.IsNullOrEmpty(referrer) && _condition.Get(referrer) ? referrer : null;
		return result;
	}
}