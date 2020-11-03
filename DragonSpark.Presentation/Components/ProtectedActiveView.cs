using AsyncUtilities;
using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components
{
	public class ProtectedActiveView<TValue> : ActiveView<TValue>
	{
		readonly Func<object, AsyncLock> _lock;

		public ProtectedActiveView() : this(Locks.Default.Get) {}

		public ProtectedActiveView(Func<object, AsyncLock> @lock) => _lock = @lock;

		protected override async Task OnParametersSetAsync()
		{
			using (await _lock(Receiver).LockAsync())
			{
				await base.OnParametersSetAsync();
			}
		}

		[Parameter]
		public object Receiver { get; set; } = default!;
	}
}