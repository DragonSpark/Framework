using JetBrains.Annotations;
using Microsoft.AspNetCore.Components;
using System;

namespace DragonSpark.Presentation.Components.Navigation
{
	public class NavigateTo : ComponentBase
	{
		[Inject, UsedImplicitly]
		protected NavigationManager Navigation { get; set; } = default!;

		[Parameter]
		public string Path { get; set; } = default!;

		[Parameter]
		public bool Forced { get; set; }

		protected override void OnInitialized()
		{
			Navigate();
		}

		protected void Navigate()
		{
			var path = Path ?? throw new InvalidOperationException("Path not provided for navigation.");
			if (path.TrimStart('/') != Navigation.ToBaseRelativePath(Navigation.Uri))
			{
				Navigation.NavigateTo(path, Forced);
			}
		}
	}
}