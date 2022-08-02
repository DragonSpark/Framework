using System.Net;

namespace DragonSpark.Application.Connections;

public class HeaderName : Text.Text
{
	protected HeaderName(HttpRequestHeader instance) : base(instance.ToString()) {}
}