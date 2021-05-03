using AspNet.Security.OAuth.Paypal;
using DragonSpark.Model.Commands;
using System;

namespace DragonSpark.Identity.PayPal
{
	sealed class ConfigureAuthentication : ICommand<PaypalAuthenticationOptions>
	{
		readonly Func<PayPalApplicationSettings> _settings;

		public ConfigureAuthentication(Func<PayPalApplicationSettings> settings) => _settings = settings;

		public void Execute(PaypalAuthenticationOptions parameter)
		{
			var settings = _settings();
			parameter.ClientId     = settings.Key;
			parameter.ClientSecret = settings.Secret;


			var authentication = settings.Authentication;
			if (authentication != null)
			{
				parameter.AuthorizationEndpoint   = authentication.AuthorizationEndpoint;
				parameter.TokenEndpoint           = authentication.TokenEndpoint;
				parameter.UserInformationEndpoint = authentication.UserInformationEndpoint;

				if (authentication.Scopes != null)
				{
					parameter.Scope.Clear();
					foreach (var scope in authentication.Scopes)
					{
						parameter.Scope.Add(scope);
					}
				}
			}
		}
	}
}