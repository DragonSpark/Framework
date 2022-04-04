using System.Net;

namespace DragonSpark.Application.Compose.Communication;

public class Header : Text.Text
{
	protected Header(HttpRequestHeader instance) : base(instance.ToString()) {}
}