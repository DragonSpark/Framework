using DragonSpark.Compose;
using DragonSpark.Contracts.Uploads;
using DragonSpark.Model;
using DragonSpark.Presentation;
using DragonSpark.Presentation.Components;
using DragonSpark.Presentation.Components.State;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Inputs;

namespace DragonSpark.SyncfusionRendering.Components.Uploads;

public abstract class UploadBase<T> : UploadComponentBase<T>, IAsyncDisposable
{
	Syncfusion.Blazor.Inputs.FileInfo?                  _current;
	protected readonly string                           _id = UniqueIdentifiers.Default;
	protected          SfUploader?                      _subject;
	protected          EventCallback<SuccessEventArgs>  _success;
	protected          EventCallback<SelectedEventArgs> _selected;
	protected          EventCallback<CancelEventArgs>   _cancel;

	protected override void OnInitialized()
	{
		_success  = Start.A.Callback<SuccessEventArgs>(OnSuccess).UpdateActivity(Receiver).Handle(Exceptions);
		_selected = Start.A.Callback<SelectedEventArgs>(OnSelected).UpdateActivity(Receiver).Handle(Exceptions);
		_cancel   = Start.A.Callback<CancelEventArgs>(OnCancel).UpdateActivity(Receiver).Handle(Exceptions);

		base.OnInitialized();

		Form.Add(new { Session });
	}

	[Parameter, EditorRequired]
	public required uint User { get; set; }

	[Parameter]
	public Guid Workspace { get; set; }
	
	[CascadingParameter]
	IActivityReceiver Receiver { get; set; } = null!;

	[Inject] INewUpload New { get; set; } = null!;

	[Inject] RequestHeader Header { get; set; } = null!;

	protected List<object> Form { get; set; } = [];

	Task OnSelected(SelectedEventArgs parameter)
	{
		switch (parameter.FilesData.Count)
		{
			case 0:
				break;
			case 1:
				_current = parameter.FilesData.Single();
				break;
			default:
				throw MultipleFilesDetectedException.Default;
		}
		return Task.CompletedTask;
	}

	protected void OnRemove(BeforeRemoveEventArgs parameter)
	{
		_current                 = null;
		parameter.CustomFormData = Form;
		parameter.CurrentRequest = Header.Get();
	}

	
	async Task OnCancel(CancelEventArgs parameter)
	{
		_current = null;
		await Cancel.On(new(new(User, new(Workspace, parameter.FileData.Name)), Stop));
		await Deactivated.Off();
	}

	protected async Task OnBefore(BeforeUploadEventArgs parameter)
	{
		var current = _current.Verify();
		parameter.CustomFormData = Form;
		parameter.CurrentRequest = await New.On(new(new(current.Name, current.MimeContentType), Stop));
		await Activated.Off();
	}

	async Task OnSuccess(SuccessEventArgs parameter)
	{
		_current = null;
		await Deactivated.On();
		switch (parameter.Operation)
		{
			case "remove":
				break;
			default:
				var name  = new WorkspacePath(Workspace, $"{Session}/{parameter.File.Name}");
				var entry = await Completed.On(new(new(User, name), Stop));
				await Uploaded.Off(entry);
				break;
		}
	}

	protected Task OnFailure(FailureEventArgs parameter) => Deactivated.Invoke();

	public ValueTask DisposeAsync()
		=> _subject?.CancelAsync(_current?.Yield().ToArray() ?? Empty.Array<Syncfusion.Blazor.Inputs.FileInfo>()).ToOperation() ?? ValueTask.CompletedTask;
}