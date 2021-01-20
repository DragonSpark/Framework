using DragonSpark.Model.Results;
using Microsoft.AspNetCore.Components;

namespace DragonSpark.Application.Navigation
{
	public sealed class CurrentRootPath : IResult<string>
	{
		readonly NavigationManager _manager;

		public CurrentRootPath(NavigationManager manager) => _manager = manager;

		public string Get() => $"/{_manager.ToBaseRelativePath(_manager.Uri)}";
	}
}