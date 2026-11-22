using DragonSpark.Application.Diagnostics;
using DragonSpark.Compose;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Exception = System.Exception;

namespace DragonSpark.Presentation.Components.Diagnostics;

public sealed class ExceptionBoundaryComponent : ErrorBoundary
{
	[Parameter]
	public required IReportedTypeAware? ReportedType { get; set; }

	[Inject] public required IExceptionLogger Exceptions { get; set; }

	protected override Task OnErrorAsync(Exception exception)
		=> Exceptions.Allocate(new(ReportedType?.Get() ?? GetType(), exception));
}