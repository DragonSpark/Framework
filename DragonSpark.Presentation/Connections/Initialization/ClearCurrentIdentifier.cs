using DragonSpark.Application.Security;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Presentation.Connections.Initialization;

sealed class ClearCurrentIdentifier : DelegatedParameterCommand<HttpContext>
{
	public ClearCurrentIdentifier(ICurrentContext parameter) : base(ClearIdentifier.Default, parameter) {}
}