using AspNet.Security.OAuth.Paypal;
using DragonSpark.Application.Compose;
using System;

namespace DragonSpark.Identity.PayPal;

public static class Extensions
{
	public static AuthenticationContext UsingPayPal(this AuthenticationContext @this) => UsingPayPal(@this, _ => {});

	public static AuthenticationContext UsingPayPal(this AuthenticationContext @this,
	                                                Action<PaypalAuthenticationOptions> configure)
		=> @this.Append(new ConfigureApplication(configure));
}