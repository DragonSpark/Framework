using DragonSpark.Application.Model;
using DragonSpark.Contracts.Uploads;
using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Model.Operations.Stop;
using Microsoft.AspNetCore.Components;

namespace DragonSpark.SyncfusionRendering.Components.Uploads;

public class UploadComponentBase<T> : DragonSpark.Presentation.Components.ComponentBase
{
	[Parameter]
	public EventCallback<T> Uploaded { get; set; }

	[Parameter]
	public required string AllowedExtensions { get; set; }

	[Parameter]
	public required string Prompt { get; set; } = "Add/Drop New File";

	[Parameter]
	public bool ShowFileList { get; set; } = true;

	[Parameter]
	public required IStopAware<UserInput<WorkspacePath>, T> Completed { get; set; }

	[Parameter]
	public required IStopAware<UserInput<WorkspacePath>> Cancel { get; set; }

	[Parameter]
	public required UploadSettings Settings { get; set; }

	[Parameter]
	public EventCallback Activated { get; set; }

	[Parameter]
	public EventCallback Deactivated { get; set; }

	[Parameter]
	public uint MaximumSize { get; set; } = DefaultMaximumUploadSize.Default;

	[Parameter]
	public Guid Session { get; set; } = Guid.NewGuid();
}