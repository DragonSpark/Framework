using AspNet.Security.OAuth.Paypal;
using DragonSpark.Application.Security.Identity.Claims.Actions;
using DragonSpark.Model.Commands;
using System;

namespace DragonSpark.Identity.PayPal
{
	sealed class ConfigureAuthentication : ICommand<PaypalAuthenticationOptions>
	{
		readonly Func<PayPalApplicationSettings> _settings;
		readonly IClaimAction                    _claim;

		public ConfigureAuthentication(Func<PayPalApplicationSettings> settings)
			: this(settings, PayIdentifierClaimAction.Default) {}

		public ConfigureAuthentication(Func<PayPalApplicationSettings> settings, IClaimAction claim)
		{
			_settings = settings;
			_claim    = claim;
		}

		public void Execute(PaypalAuthenticationOptions parameter)
		{
			var settings = _settings();
			parameter.ClientId     = settings.Key;
			parameter.ClientSecret = settings.Secret;

			var authentication = settings.Authentication;
			parameter.AuthorizationEndpoint   = authentication.AuthorizationEndpoint;
			parameter.TokenEndpoint           = authentication.TokenEndpoint;
			parameter.UserInformationEndpoint = authentication.UserInformationEndpoint;

			_claim.Execute(parameter.ClaimActions);

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

	public sealed class PayPalClaimNamespace : Text.Text
	{
		public static PayPalClaimNamespace Default { get; } = new PayPalClaimNamespace();

		PayPalClaimNamespace() : base("urn:paypal") {}
	}

	public sealed class PayIdentifierClaimAction : ClaimAction
	{
		public static PayIdentifierClaimAction Default { get; } = new PayIdentifierClaimAction();

		PayIdentifierClaimAction() : base(PayIdentifier.Default, "payer_id") {}
	}

	public class PayPalClaim : Text.Text
	{
		protected PayPalClaim(string name) : base($"{PayPalClaimNamespace.Default}:{name}") {}
	}

	public sealed class PayIdentifier : PayPalClaim
	{
		public static PayIdentifier Default { get; } = new PayIdentifier();

		PayIdentifier() : base(nameof(PayIdentifier).ToLower()) {}
	}
}