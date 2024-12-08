using DragonSpark.Application.AspNet.Compose;
using DragonSpark.Application.Compose;

namespace DragonSpark.SendGrid;

public static class Extensions
{
	public static ApplicationProfileContext WithSendGrid(this ApplicationProfileContext @this)
		=> @this.Append(Registrations.Default);
}