﻿using DragonSpark.Application.Navigation;
using DragonSpark.Presentation.Components.Navigation;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;


namespace DragonSpark.Presentation.Components.Security
{
	/// <summary>
	/// ATTRIBUTION: https://blog.vfrz.fr/blazor-redirect-non-authenticated-user/
	/// </summary>
	public sealed class RedirectToLogin : NavigateTo
	{
		public RedirectToLogin() => Forced = true;

		[Parameter]
		public string FormatPath { get; set; } = LoginPathTemplate.Default;

		[Inject]
		ILogger<RedirectToLogin> Logger { get; set; } = default!;

		[Inject]
		CurrentRootPath CurrentPath { get; set; } = default!;

		protected override void OnInitialized()
		{
			Path = new TemplatedPath(FormatPath).Get(CurrentPath.Get());

			Logger.LogDebug("Unauthorized resource '{Uri}' detected.  Redirecting to: {Redirect}",
			                Navigation.Uri, Path);
			base.OnInitialized();
		}
	}
}