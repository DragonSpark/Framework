using DragonSpark.Compose;
using DragonSpark.Presentation.Components.Content;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components
{
	public class DataList<T> : RadzenDataList<T>, IRefreshAware
	{
		[CascadingParameter]
		IRefreshContainer? Container
		{
			get => _container.Verify();
			set
			{
				if (_container != value)
				{
					_container?.Remove.Execute(this);

					_container = value;

					_container?.Add.Execute(this);
				}
			}
		}	IRefreshContainer? _container = default!;

		public Task Get() => Reload();

		public override void Dispose()
		{
			Container = null;
			base.Dispose();
		}
	}
}