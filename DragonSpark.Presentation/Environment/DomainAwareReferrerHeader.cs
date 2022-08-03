using DragonSpark.Compose;
using DragonSpark.Text;

namespace DragonSpark.Presentation.Environment;

public class DomainAwareReferrerHeader : ConditionAwareReferrerHeader
{
	public DomainAwareReferrerHeader(string domain) : base(new ContainsText(domain).Then().Inverse().Out()) {}
}