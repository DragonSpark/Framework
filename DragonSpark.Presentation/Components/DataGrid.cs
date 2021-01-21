﻿using DragonSpark.Application.Runtime;
using DragonSpark.Compose;
using DragonSpark.Presentation.Components.Callbacks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen.Blazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components
{
	/// <summary>
	/// https://forum.radzen.com/t/issues-with-radzengrid-dispose/5265
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class DataGrid<T> : RadzenGrid<T>
	{
		[CascadingParameter]
		IActivityReceiver? Receiver
		{
			get => _receiver;
			set
			{
				if (_receiver != value)
				{
					_receiver = value;

					Refresh = _receiver != null
						          ? Exceptions.Bind(base.Reload).Using(_receiver).UpdateActivity().Get()
						          : null;
				}
			}
		}

		IActivityReceiver? _receiver;

		[Inject]
		IExceptions Exceptions { get; set; } = default!;

		EventCallback? Refresh { get; set; }

		public override Task Reload() => Refresh?.InvokeAsync() ?? base.Reload();

		public override void Dispose()
		{
			GC.SuppressFinalize(this);
			InvokeAsync(Disposing);
		}

		async ValueTask InvokeVoid(string name, object parameter)
		{
			try
			{
				await JSRuntime.InvokeVoidAsync(name, parameter);
			}
			catch
			{
				// Should log, etc.
			}
		}

		async Task Disposing()
		{
			if (ContextMenu.HasDelegate)
				await InvokeVoid("Radzen.removeContextMenu", UniqueID);
			if (MouseEnter.HasDelegate)
				await InvokeVoid("Radzen.removeMouseEnter", UniqueID);
			if (MouseLeave.HasDelegate)
			{
				await InvokeVoid("Radzen.removeMouseLeave", UniqueID);

				var columns = GetType()
				              .GetField("columns")
				              .Verify()
				              .GetValue(this)
				              .Verify()
				              .To<List<RadzenGridColumn<T>>>();

				var id = $"popup{UniqueID}";

				foreach (var column in columns.Where(c => c.Visible))
				{
					await InvokeVoid("Radzen.destroyPopup", id + column.GetFilterProperty());
				}
			}
		}
	}
}