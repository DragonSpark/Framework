using System;
using System.Threading.Tasks;
using DragonSpark.Application.Mobile.Maui.Model.Commands;
using Microsoft.Maui.ApplicationModel;

namespace DragonSpark.Application.Mobile.Maui.Navigation;

public sealed class Launch : AsynchronousCommand<Uri>
{
	public static Launch Default { get; } = new();

	Launch() : base(x => x is not null ? Launcher.OpenAsync(x) : Task.CompletedTask) {}
}