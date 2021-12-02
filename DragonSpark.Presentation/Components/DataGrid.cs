using DragonSpark.Reflection.Members;
using Microsoft.JSInterop;
using Radzen.Blazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components;

/// <summary>
/// https://forum.radzen.com/t/issues-with-radzengrid-dispose/5265
/// </summary>
/// <typeparam name="T"></typeparam>
public class DataGrid<T> : RadzenGrid<T>
{
	protected override void OnParametersSet()
	{
		if (Visible && LoadData.HasDelegate && Data == null)
		{
			Visible = false;
			InvokeAsync(Reload);
		}
		else
		{
			Visible |= Data != null;
		}
		base.OnParametersSet();
	}

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

			var columns = ColumnsField.Default.Get(this);
			var id      = $"popup{UniqueID}";

			foreach (var column in columns.Where(c => c.Visible))
			{
				await InvokeVoid("Radzen.destroyPopup", id + column.GetFilterProperty());
			}
		}
	}

	sealed class ColumnsField : FieldAccessor<DataGrid<T>, List<RadzenGridColumn<T>>>
	{
		public static ColumnsField Default { get; } = new();

		ColumnsField() : base("columns") {}
	}
}