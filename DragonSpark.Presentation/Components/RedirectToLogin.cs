﻿using JetBrains.Annotations;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using System.Net;

namespace DragonSpark.Presentation.Components
{
	/// <summary>
	/// ATTRIBUTION: https://blog.vfrz.fr/blazor-redirect-non-authenticated-user/
	/// </summary>
	public sealed class RedirectToLogin : NavigateTo
	{
		public RedirectToLogin() => Forced = true;

		[Parameter]
		public string FormatPath { get; set; } = "Identity/Account/Login?ReturnUrl={0}";

		[Inject, UsedImplicitly]
		ILogger<RedirectToLogin> Logger { get; set; } = default!;

		protected override void OnInitialized()
		{
			var @return = WebUtility.UrlEncode($"/{Navigation.ToBaseRelativePath(Navigation.Uri)}");

			Path = string.Format(FormatPath, @return);

			Logger.LogDebug("Unauthorized resource '{Uri}' detected.  Redirecting to: {Redirect}",
			                Navigation.Uri, Path);
			base.OnInitialized();
		}
	}
}