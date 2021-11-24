using DragonSpark.Model.Commands;
using DragonSpark.Presentation.Security;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Presentation.Connections.Initialization;

sealed class ClearCurrentIdentifier : DelegatedParameterCommand<HttpContext>
{
	public ClearCurrentIdentifier(ICurrentContext parameter) : base(ClearIdentifier.Default, parameter) {}
}