using DragonSpark.Application.AspNet.Compose;

namespace DragonSpark.SendGrid;

public static class Extensions
{
	public static ApplicationProfileContext WithSendGrid(this ApplicationProfileContext @this)
		=> @this.Append(Registrations.Default);
}