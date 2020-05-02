using JetBrains.Annotations;
using Microsoft.AspNetCore.Components;
using System;

namespace DragonSpark.Presentation.Components
{
	public class NavigateTo : ComponentBase
	{
		[Inject, UsedImplicitly]
		protected NavigationManager Navigation { get; set; }

		[Parameter]
		public string Path { get; set; }

		[Parameter]
		public bool Forced { get; set; }

		protected override void OnInitialized()
		{
			if (Path.TrimStart('/') != Navigation.ToBaseRelativePath(Navigation.Uri))
			{
				Navigation.NavigateTo(Path ?? throw new InvalidOperationException("Path not provided for navigation."),
				                      Forced);
			}
		}
	}
}