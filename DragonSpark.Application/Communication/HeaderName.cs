using System.Net;

namespace DragonSpark.Application.Communication;

public class HeaderName : Text.Text
{
	protected HeaderName(HttpRequestHeader instance) : base(instance.ToString()) {}
}